using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Contest.Core.Converters;

namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Expose methods for CRUD action on T base on Sql database for persistance
    /// </summary>
    /// <typeparam name="T">Type of repository object</typeparam>
    /// <typeparam name="TI">Type of interface object. Same object if he haven't interface. DataMember and Contract have to be specified on TI type</typeparam>
    public class SqlRepository<T, TI> : ISqlRepository<TI> where T : class, TI
        where TI : class, IQueryable
    {
        private readonly IList<string> _queryList;

        private static string DatabasePath { get { return ConfigurationManager.AppSettings["DatabasePath"]; } }

        public IUnitOfWorks UnitOfWorks { get; set; }

        internal ISqlBuilder<T, TI> SqlBuilder { get; set; }

        internal IBusinessObjectFactory<TI> BoFactory { get; set; }

        internal IConverter Converter { get; set; }

        internal IDataContext<TI> Context { get; set; }

        public IList<string> QueryList
        {
            get { return _queryList; }
        }

        /// <summary>
        /// Create a new Repository
        /// </summary>
        public SqlRepository()
        {
            _queryList = new List<string>();
            SqlBuilder = new SqlBuilder<T,TI>();
            Converter = Converters.Converter.Instance;
            Context = new DataContext<TI>();
        }

        /// <summary>
        /// Prepare request for create table on managed object
        /// </summary>
        public void CreateTable()
        {
            var request = SqlBuilder.CreateTable();
            if (UnitOfWorks == null) QueryList.Add(request);
            else UnitOfWorks.AddRequest(request);
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
            IList<Tuple<string, object, object[]>> arg;
            var request = SqlBuilder.Insert(item, out arg);
            SetValueAndAppendRequest(request, arg);
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
            IList<Tuple<string, object, object[]>> arg;
            var request = SqlBuilder.Update(item, out arg);
            SetValueAndAppendRequest(request, arg);
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
            IList<Tuple<string, object, object[]>> arg;
            var request = SqlBuilder.Delete(item, out arg);
            SetValueAndAppendRequest(request, arg);
        }

        private string SetValue(string request, IEnumerable<Tuple<string, object, object[]>> arg)
        {
            return arg.Aggregate(request, (current, tuple) => current.Replace("@" + tuple.Item1 + "@", ToSqlValue(tuple.Item2, tuple.Item3)));
        }

        public string ToSqlValue(object obj, object[] attr)
        {
            if (obj == null) return "NULL";
            var value = Converter.Convert(obj, attr);
            var objectType = obj.GetType();
            if (objectType == typeof(ushort)
                || objectType == typeof(short)
                || objectType == typeof(uint)
                || objectType == typeof(int)
                || objectType == typeof(ulong)
                || objectType == typeof(long)
                || objectType == typeof(float)) return value;

            return string.Concat("'", value.Replace("'", "''"), "'");
        }

        private void SetValueAndAppendRequest(string request, IEnumerable<Tuple<string, object, object[]>> arg)
        {
            request = SetValue(request, arg);
            if (UnitOfWorks == null) QueryList.Add(request);
            else UnitOfWorks.AddRequest(request);
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
            string request = PrepareSqlRequest(predicate);

            //Execute
            using (var db = Sqlite.OpenDatabase(DatabasePath))
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandText = request;
                cmd.ExecuteNonQuery();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = BoFactory != null
                                ? BoFactory.Create(reader)
                                : SqlBuilder.CreateInstance(reader);

                        //Update context
                        if (!Context.IsExist(item)) Context.Insert(item);
                    }
                    reader.Close();
                }
            }

            //Return response
            return Context.Find(predicate.Compile());
        }

        public string PrepareSqlRequest(Expression<Func<TI, bool>> predicate)
        {
            //Prepare request
            IList<Tuple<string, object, object[]>> arg;
            var request = SqlBuilder.Select(predicate, out arg);
            request = SetValue(request, arg);
            return request;
        }

        /// <summary>
        /// Clear local cache
        /// </summary>
        public void ClearCache()
        {
            Context.Clear();
        }

        /// <summary>
        /// Persist all changes
        /// </summary>
        public void Commit()
        {
            if (UnitOfWorks != null) throw new NotSupportedException("Current repository is linked to unit of works.");
            Sqlite.Execute(DatabasePath, QueryList);
        }

        /// <summary>
        /// Undo all changes making after last commit
        /// </summary>
        public void RollBack()
        {
            if (UnitOfWorks != null) throw new NotSupportedException("Current repository is linked to unit of works.");
            QueryList.Clear();
        }
    }
}
