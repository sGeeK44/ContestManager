using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Contest.Core.DataStore.Sqlite
{
    public class SqliteDataStore : SqlDataStoreBase
    {
        private const string INTEGER_COLUMN_VALUE = "integer";
        private const string REAL_COLUMN_VALUE = "real";
        private const string TEXT_COLUMN_VALUE = "text";
        private IDbConnection _databaseConnection;

        public SqliteDataStore(string filePath = null)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Determine default Sql column type when .net is not managed
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDefaultColumnType() { return TEXT_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store string .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetStringColumnType() { return TEXT_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store decimal .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDecimalColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store float .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetFloatColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store double .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDoubleColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store long .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetLongColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store ulong .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedLongColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store uint .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedIntColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store int .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetIntColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store short .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetShortColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store ushort .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedShortColumnType() { return INTEGER_COLUMN_VALUE; }

        public string FilePath { get; set; }

        public override void OpenDatabase()
        {
            using (var factory = new SQLiteFactory())
            {
                _databaseConnection = factory.CreateConnection();
                if (_databaseConnection == null) throw new InvalidProgramException("Failed to create database connector.");
                _databaseConnection.ConnectionString = string.Format(@"Data Source={0}; Pooling=false; FailIfMissing=false;", FilePath);
            }
            _databaseConnection.Open();
        }

        public override void Execute(IList<string> requestList)
        {
            if (requestList == null || requestList.Count == 0) return;
            
            using (var dbTransaction = _databaseConnection.BeginTransaction())
            try
            {
                using (var cmd = _databaseConnection.CreateCommand())
                foreach (var cdmString in requestList)
                {
                    cmd.Transaction = dbTransaction;
                    cmd.CommandText = cdmString;
                    cmd.ExecuteNonQuery();
                }
                dbTransaction.Commit();
                requestList.Clear();
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                throw;
            }
        }

        public override void CloseDatabase()
        {
            if (_databaseConnection.State != ConnectionState.Closed) _databaseConnection.Close();
            _databaseConnection.Dispose();
        }

        public override IDataReader Execute(string request)
        {
            using (var cmd = _databaseConnection.CreateCommand())
            {
                cmd.CommandText = request;
                cmd.ExecuteNonQuery();
                return cmd.ExecuteReader();
            }
        }
    }
}
