using System.Configuration;
using Contest.Core.DataStore.Sql.BusinessObjectFactory;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sqlite;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public class SqlRepositoryBase<T, TI> : SqlRepository<TI> where TI : class,
        IQueryable where T : class, TI
    {
        private static string DatabasePath { get { return ConfigurationManager.AppSettings["DatabasePath"]; } }

        public SqlRepositoryBase()
            : base(new SqliteDataStore(DatabasePath), new SqlQueryFactory<T, TI>(new SqliteStrategy()), new SqlSerializer<T, TI>(), new DataContext<TI>())
        {
            SqlDataStore.OpenDatabase();
        }
    }
}
