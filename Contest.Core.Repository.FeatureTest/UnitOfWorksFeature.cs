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
        public DbConnection DatabaseConnection { get; set; }
        public SqliteDataStore DataStore { get; set; }
        public ISqlUnitOfWorks UnitOfWork { get; set; }

        [TestCase]
        public void CreateTable()
        {
            CreateNewDataStore("UnitsOfWorksFeature.CreateTable.db");
            var repository = CreateRepository<MainEntity, IEntity>();

            repository.CreateTable();
            UnitOfWork.Commit();

            AssertMainEntityTable();
        }

        [TestCase]
        public void InsertItem()
        {
            CreateNewDataStore("UnitsOfWorksFeature.InsertItem.db");
            ExecuteQuery("CREATE TABLE MainEntity (Key text primary key, BasicField text, OVERRIDE_COLUMN_NAME text, IntegerField integer);");
            var newItem = new MainEntity
            {
                Key = Guid.NewGuid(),
                BasicField = "Value_1",
                OverrideColumnName = "Value_2",
                IntegerField = 1
            };
            var repository = CreateRepository<MainEntity, IEntity>();

            repository.Insert(newItem);
            UnitOfWork.Commit();

            Assert.AreEqual(1, CountMainEntityRow());
        }

        [TestCase]
        public void ReadItem()
        {
            CreateNewDataStore("UnitsOfWorksFeature.ReadItem.db");
            ExecuteQuery("CREATE TABLE MainEntity (Key text primary key, BasicField text, OVERRIDE_COLUMN_NAME text, IntegerField integer);");
            ExecuteQuery("INSERT INTO MainEntity (Key, BasicField, OVERRIDE_COLUMN_NAME, IntegerField) VALUES ('62B819A7-E20D-4728-94FD-2A10C62EEC2E', 'Value_1', 'Value_2', 1);");
            var expectedItem = new MainEntity
            {
                Key = new Guid("62B819A7-E20D-4728-94FD-2A10C62EEC2E"),
                BasicField = "Value_1",
                OverrideColumnName = "Value_2",
                IntegerField = 1
            };
            var repository = CreateRepository<MainEntity, IEntity>();

            var databaseResult = repository.Find(_ => _.Key == expectedItem.Key).First();
            Assert.AreEqual(expectedItem, databaseResult);
        }

        [TestCase]
        public void DeleteItem()
        {
            CreateNewDataStore("UnitsOfWorksFeature.DeleteItem.db");
            ExecuteQuery("CREATE TABLE MainEntity (Key text primary key, BasicField text, OVERRIDE_COLUMN_NAME text, IntegerField integer);");
            ExecuteQuery("INSERT INTO MainEntity (Key, BasicField, OVERRIDE_COLUMN_NAME, IntegerField) VALUES ('62B819A7-E20D-4728-94FD-2A10C62EEC2E', 'Value_1', 'Value_2', 1);");
            var itemToDelete = new MainEntity
            {
                Key = new Guid("62B819A7-E20D-4728-94FD-2A10C62EEC2E"),
                BasicField = "Value_1",
                OverrideColumnName = "Value_2",
                IntegerField = 1
            };
            var repository = CreateRepository<MainEntity, IEntity>();

            repository.Delete(itemToDelete);
            UnitOfWork.Commit();

            CountMainEntityRow();

            Assert.AreEqual(0, CountMainEntityRow());
        }

        [TestCase]
        public void ReadItemWithManyToOneReference()
        {
            const string entityGuid = "62B819A7-E20D-4728-94FD-2A10C62EEC2E";
            const string referenceGuid = "0A9E8A83-2DD2-4912-AE6C-25B9770D0F30";

            CreateNewDataStore("UnitsOfWorksFeature.ReadItemWithManyToOneReference.db");
            InitReferenceItemDatabase(entityGuid, referenceGuid);

            var oneToMany = new OneToManyEntity
            {
                Id = new Guid(entityGuid),
                EntityList = new List<ManyToOneEntity>()
            };
            var manyToOne = new ManyToOneEntity
            {
                Id = new Guid(referenceGuid),
                OneToManyEntityId = new Guid(entityGuid)
            };
            oneToMany.EntityList.Add(manyToOne);

            CreateRepository<ManyToOneEntity, ManyToOneEntity>();
            var oneToManyEntityRepository = CreateRepository<OneToManyEntity, OneToManyEntity>();

            var guid = new Guid(entityGuid);
            var databaseResult = oneToManyEntityRepository.Find(_ => _.Id == guid).First();
            Assert.AreEqual(oneToMany, databaseResult);
        }

        [TestCase]
        public void ReadItemWithOneToManyReference()
        {
            const string entityGuid = "62B819A7-E20D-4728-94FD-2A10C62EEC2E";
            const string referenceGuid = "0A9E8A83-2DD2-4912-AE6C-25B9770D0F30";

            CreateNewDataStore("UnitsOfWorksFeature.ReadItemWithOneToManyReference.db");
            InitReferenceItemDatabase(entityGuid, referenceGuid);

            var oneToMany = new OneToManyEntity
            {
                Id = new Guid(entityGuid),
                EntityList = new List<ManyToOneEntity>()
            };
            var manyToOne = new ManyToOneEntity
            {
                Id = new Guid(referenceGuid),
                OneToManyEntityId = new Guid(entityGuid),
                Entity = oneToMany
            };
            oneToMany.EntityList.Add(manyToOne);

            var repository = CreateRepository<ManyToOneEntity, ManyToOneEntity>();
            CreateRepository<OneToManyEntity, OneToManyEntity>();

            var guid = new Guid(referenceGuid);
            var databaseResult = repository.Find(_ => _.Id == guid).First();
            Assert.AreEqual(manyToOne, databaseResult);
        }

        private void InitReferenceItemDatabase(string entityGuid, string referenceGuid)
        {
            ExecuteQuery("CREATE TABLE OneToManyEntity (Id text primary key);");
            ExecuteQuery("CREATE TABLE ManyToOneEntity (Id text primary key, OneToManyEntityId text);");
            ExecuteQuery(string.Format("INSERT INTO OneToManyEntity (Id) VALUES ('{0}');", entityGuid));
            ExecuteQuery(string.Format("INSERT INTO ManyToOneEntity (Id, OneToManyEntityId) VALUES ('{0}', '{1}');",
                referenceGuid, entityGuid));
        }

        private void ExecuteQuery(string query)
        {
            using (var dbTransaction = DatabaseConnection.BeginTransaction())
            using (var cmd = DatabaseConnection.CreateCommand())
            {
                cmd.Transaction = dbTransaction;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                dbTransaction.Commit();
            }
        }

        private int CountMainEntityRow()
        {
            using (IDbCommand command = new SQLiteCommand())
            {
                command.Connection = DatabaseConnection;
                var sql = string.Format("SELECT * FROM {0}", MainEntity.TableName);
                command.CommandText = sql;
                var count = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public SqlRepository<TI> CreateRepository<T, TI>()
            where TI : class, IQueryable
            where T : class, TI
        {
            var sqlBuilder = new SqlQueryFactory<T, TI>(new SqliteStrategy());
            var boFactory = new SqlSerializer<T, TI>();
            var context = new DataContext<TI>();

            var repository = new SqlRepository<TI>(DataStore, sqlBuilder, boFactory, context);
            UnitOfWork.AddRepository(repository);
            return repository;
        }

        private void CreateNewDataStore(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);
            DatabaseConnection = GetNewConnection(filePath);

            DataStore = new SqliteDataStore(filePath);
            DataStore.OpenDatabase();

            UnitOfWork = new SqlUnitOfWorks(DataStore);
        }

        public DbConnection GetNewConnection(string filePath)
        {
            using (var factory = new SQLiteFactory())
            {
                var databaseConnection = factory.CreateConnection();
                if (databaseConnection == null) throw new Exception();

                databaseConnection.ConnectionString = string.Format(@"Data Source={0}; Pooling=false; FailIfMissing=false;", filePath);
                databaseConnection.Open();
                return databaseConnection;
            }
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

        public void AssertMainEntityTable()
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
