using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Contest.Core.DataStore.Sql;
using Contest.Core.DataStore.Sql.SqlQuery;

namespace Contest.Core.DataStore.Sqlite
{
    public class SqliteDataStore : ISqlDataStore
    {
        private IDbConnection _databaseConnection;

        public SqliteDataStore(string filePath = null)
        {
            FilePath = filePath;
        }

        public string FilePath { get; set; }

        public void OpenDatabase()
        {
            using (var factory = new SQLiteFactory())
            {
                _databaseConnection = factory.CreateConnection();
                if (_databaseConnection == null) throw new InvalidProgramException("Failed to create database connector.");
                _databaseConnection.ConnectionString = string.Format(@"Data Source={0}; Pooling=false; FailIfMissing=false;", FilePath);
            }
            _databaseConnection.Open();
        }

        public IDataReader Execute(ISqlQuery request)
        {
            using (var cmd = _databaseConnection.CreateCommand())
            {
                cmd.CommandText = request.ToStatement();
                cmd.ExecuteNonQuery();
                return cmd.ExecuteReader();
            }
        }

        public void Execute(IList<ISqlQuery> requestList)
        {
            if (requestList == null || requestList.Count == 0) return;
            
            using (var dbTransaction = _databaseConnection.BeginTransaction())
            try
            {
                using (var cmd = _databaseConnection.CreateCommand())
                foreach (var request in requestList)
                {
                    cmd.Transaction = dbTransaction;
                    cmd.CommandText = request.ToStatement();
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

        public void CloseDatabase()
        {
            if (_databaseConnection.State != ConnectionState.Closed) _databaseConnection.Close();
            _databaseConnection.Dispose();
        }
    }
}
