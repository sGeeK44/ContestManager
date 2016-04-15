using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql;
using Contest.Core.DataStore.Sql.BusinessObjectFactory;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sqlite;
namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Expose methods for CRUD action on T base on Sql database for persistance
    /// </summary>
    /// <typeparam name="T">Type of repository object</typeparam>
    public class SqlRepository<T> : SqlRepository<T, T> where T : class, IQueryable
    {
        public SqlRepository(ISqlDataStore dataStore) : base(dataStore) { }
    }
    
    /// <summary>
    /// Expose methods for CRUD action on T base on Sql database for persistance
    /// </summary>
    /// <typeparam name="T">Type of repository object</typeparam>
    /// <typeparam name="TI">Type of interface object. Same object if he haven't interface. DataMember and Contract have to be specified on TI type</typeparam>
    public class SqlRepository<T, TI> : ISqlRepository<TI> where T : class, TI
        where TI : class, IQueryable
    {
        private static string DatabasePath { get { return ConfigurationManager.AppSettings["DatabasePath"]; } }

        private ISqlDataStore SqlDataStore { get; set; }

        public ISqlUnitOfWorks UnitOfWorks { get; set; }

        internal ISqlQueryFactory<TI> SqlQueryFactory { get; set; }

        internal IBusinessObjectFactory<TI> BoFactory { get; set; }

        internal IDataContext<TI> Context { get; set; }
        
        /// <summary>
        /// Create a new Repository
        /// </summary>
        public SqlRepository(ISqlDataStore dataStore)
        {
            SqlQueryFactory = new SqlQueryFactory<T,TI>(new SqliteStrategy());
            Context = new DataContext<TI>();
            BoFactory = new SqlSerializer<T, TI>();
            SqlDataStore = dataStore;
        }

        /// <summary>
        /// Prepare request for create table on managed object
        /// </summary>
        public void CreateTable()
        {
            var request = SqlQueryFactory.CreateTable();
            AppendRequest(request);
        }

        /// <summary>
        /// Insert new item from repository
        /// </summary>
        /// <param name="item">New item to insert</param>
        public void Insert(TI item)
        {
            // Update local cache
            Context.Insert(item);

            //Build request and append it for next commit
            var request = SqlQueryFactory.Insert(item);
            AppendRequest(request);
        }

        /// <summary>
        /// Try to update before make an Insert of item from repository
        /// </summary>
        /// <param name="item">New or Existing item to insert/Update</param>
        public void InsertOrUpdate(TI item)
        {
            if (Context.IsExist(item)) Update(item);
            else Insert(item);
        }

        /// <summary>
        /// Update existing item from repository
        /// </summary>
        /// <param name="item">Existing item to update</param>
        public void Update(TI item)
        {
            //Update local cache
            Context.Update(item);

            // Build request and append it for next commit
            var request = SqlQueryFactory.Update(item);
            AppendRequest(request);
        }

        /// <summary>
        /// Remove existing item from repository
        /// </summary>
        /// <param name="item">Old item to delete</param>
        public void Delete(TI item)
        {
            //Update local cache
            Context.Delete(item);

            //Build request and append it for next commit
            var request = SqlQueryFactory.Delete(item);
            AppendRequest(request);
        }
        
        private void AppendRequest(ISqlQuery request)
        {
            var sqlDataStore = UnitOfWorks != null ? UnitOfWorks.SqlDataStore : SqlDataStore;
            sqlDataStore.AddRequest(request);
        }

        /// <summary>
        /// Search item in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <retur>First item found or default(T)</retur>
        public TI FirstOrDefault(Expression<Func<TI, bool>> predicate)
        {
            //Search in local cache
            var item = Context.FirstOrDefault(predicate.Compile());

            //If we get one return it, else execute query and first or default.
            return item ?? Find(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Search items in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        public IList<TI> Find(Expression<Func<TI, bool>> predicate)
        {
            var request = SqlQueryFactory.Select(predicate);
            var result = SqlDataStore.Execute(request);

            while (result.Read())
            {
                var item = BoFactory.Convert(result);

                //Update context
                if (!Context.IsExist(item)) Context.Insert(item);

                TryFillOneToManyReference(item);
            }
            result.Close();

            //Return response
            return Context.Find(predicate.Compile());
        }

        public void TryFillOneToManyReference(TI item)
        {
            if (UnitOfWorks == null) return;

            
        }

        /// <summary>
        /// Clear local cache
        /// </summary>
        public void ClearCache()
        {
            Context.Clear();
        }
    }
}