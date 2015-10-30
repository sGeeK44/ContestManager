﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Contest.Core.Database
{
    public class Sqlite
    {
        public static IDbConnection OpenDatabase(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            using (var factory = new SQLiteFactory())
            {
                IDbConnection dbConn = factory.CreateConnection();
                if (dbConn == null) throw new InvalidProgramException("Failed to open database.");
                dbConn.ConnectionString = string.Format(@"Data Source={0}; Pooling=false; FailIfMissing=false;", filePath);
                dbConn.Open();
                return dbConn;
            }
        }

        public static void Execute(string database, IList<string> requestList)
        {
            if (requestList == null || requestList.Count == 0) return;

            using (IDbConnection dbConnection = OpenDatabase(database))
            using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                try
                {
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        foreach (string cdmString in requestList)
                        {
                            cmd.Transaction = dbTransaction;
                            cmd.CommandText = cdmString;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbTransaction.Commit();
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
                finally
                {
                    requestList.Clear();
                }
            }
        }

        public static void CloseDatabase(IDbConnection db)
        {
            if (db == null) return;
            if (db.State != ConnectionState.Closed) db.Close();
            db.Dispose();
        }
    }
}
