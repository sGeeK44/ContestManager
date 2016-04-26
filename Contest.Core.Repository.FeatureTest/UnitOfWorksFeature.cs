using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Contest.Core.DataStore.Sql.BusinessObjectFactory;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sqlite;
using Contest.Core.Repository.Sql;
using NUnit.Framework;

namespace Contest.Core.Repository.FeatureTest
{
    [TestFixture]
    public class UnitOfWorksFeature
    {
        private const string FILE_PATH = "UnitsOfWorksFeature.db";

        public DbConnection DatabaseConnection { get; set; }
        public SqlRepository<IEntity> Repository { get; set; }
        public IUnitOfWorks UnitOfWork { get; set; }

        [SetUp]
        public void Init()
        {
            var dataStore = CreateDataStore();
            var context = new DataContext<IEntity>();
            var sqlBuilder = new SqlQueryFactory<MainEntity, IEntity>(new SqliteStrategy());
            var boFactory = new SqlSerializer<MainEntity, IEntity>();

            var unitOfWork = new SqlUnitOfWorks(dataStore);
            Repository = new SqlRepository<IEntity>(dataStore, sqlBuilder, boFactory, context)
            {
                UnitOfWorks = unitOfWork
            };
            UnitOfWork = unitOfWork;

        }

        private SqliteDataStore CreateDataStore()
        {
            if (File.Exists(FILE_PATH)) File.Delete(FILE_PATH);
            DatabaseConnection = GetNewConnection();

            var dataStore = new SqliteDataStore(FILE_PATH);
            dataStore.OpenDatabase();
            return dataStore;
        }

        public DbConnection GetNewConnection()
        {
            using (var factory = new SQLiteFactory())
            {
                var databaseConnection = factory.CreateConnection();
                if (databaseConnection == null) throw new Exception();

                databaseConnection.ConnectionString = string.Format(@"Data Source={0}; Pooling=false; FailIfMissing=false;", FILE_PATH);
                databaseConnection.Open();
                return databaseConnection;
            }
        }

        [TestCase]
        public void CreateTable()
        {
            Repository.CreateTable();
            UnitOfWork.Commit();

            ValidateMainEnityTable();
        }

        private bool IsTableExiste(string tableName)
        {
            using (IDbCommand command = new SQLiteCommand())
            {
                command.Connection = DatabaseConnection;
                var sql = string.Format("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{0}'", tableName);
                command.CommandText = sql;
                var count = Convert.ToInt32(command.ExecuteScalar());

                return (count > 0);
            }
        }

        private List<object[]> GetFieldList(string tableName)
        {
            var fieldData = new List<object[]>();
            using (IDbCommand command = new SQLiteCommand())
            {
                command.Connection = DatabaseConnection;
                command.CommandText = string.Format("PRAGMA table_info({0})", tableName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var values = new object[6];
                        reader.GetValues(values);
                        fieldData.Add(values);
                    }
                }
            }
            return fieldData;
        }

        public void ValidateMainEnityTable()
        {
            Assert.IsTrue(IsTableExiste(MainEntity.TableName));

            var databaseFieldList = GetFieldList(MainEntity.TableName);

            Assert.AreEqual(databaseFieldList.Count, MainEntity.Fields.Count);

            foreach (var entityField in MainEntity.Fields)
            {
                var databaseField = databaseFieldList.First(_ => IsSameColumn(_, entityField.ColumnName));
                Assert.AreEqual(GetColumnType(databaseField), entityField.ColumnType);
                Assert.AreEqual(IsPrimaryKey(databaseField), entityField.IsPrimaryKey);
            }
        }

        public static bool IsSameColumn(IList<object> databaseResult, string columnName)
        {
            return string.Equals(GetColumnName(databaseResult), columnName, StringComparison.InvariantCulture);
        }

        public static int GetColumnId(IList<object> databaseResult)
        {
            return Convert.ToInt32(databaseResult[0]);
        }

        public static string GetColumnName(IList<object> databaseResult)
        {
            return databaseResult[1].ToString();
        }

        public static string GetColumnType(IList<object> databaseResult)
        {
            return databaseResult[2].ToString();
        }

        public static bool CanBeNull(IList<object> databaseResult)
        {
            return Convert.ToBoolean(databaseResult[3]);
        }

        public static string GetDefaultColumnValue(IList<object> databaseResult)
        {
            return databaseResult[4].ToString();
        }

        public static bool IsPrimaryKey(IList<object> databaseResult)
        {
            return Convert.ToBoolean(databaseResult[5]);
        }
    }
}
