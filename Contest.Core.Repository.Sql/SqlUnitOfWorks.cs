using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql;

namespace Contest.Core.Repository.Sql
{
    public class SqlUnitOfWorks : ISqlUnitOfWorks
    {
        private readonly IDictionary<Type, object> _repositoryList = new Dictionary<Type, object>();

        public ISqlDataStore SqlDataStore { get; private set; }

        public SqlUnitOfWorks(ISqlDataStore sqlDataStore)
        {
            SqlDataStore = sqlDataStore;
        }
        
        /// <summary>
        /// Get boolean to know if specified repository is already present in current unit of work
        /// </summary>
        /// <typeparam name="T">Type of object managed by repository</typeparam>
        /// <param name="repository">Repository to test</param>
        /// <returns>True if repository is already present, false else</returns>
        public bool ConstainsRepository<T>(ISqlRepository<T> repository) where T : class
        {
            //Check if we haven't a repository for this object type.
            return _repositoryList.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Create link between specified repository and current unit of works
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        public void AddRepository<T>(ISqlRepository<T> repository)
            where T : class
        {
            if (repository == null) throw new ArgumentNullException("repository");

            //Check if we haven't a repository for this object type.
            if (ConstainsRepository(repository)) throw new ArgumentException(string.Format("Unit of works contains already this type of repository. Type:{0}", typeof(T)));

            //Check if specified repository isn't linked to an other unit of works.
            if (repository.UnitOfWorks != null) throw new ArgumentException("Specified repository is already linked to another repository.");

            //All is find, create liason between repo and unit.
            repository.UnitOfWorks = this;
            _repositoryList.Add(typeof(T), repository);
        }

        /// <summary>
        /// Insert new item from repository
        /// </summary>
        /// <param name="item">New item to insert</param>
        public void Insert<T>(T item) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof (T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));
            
            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            repo.Insert(item);
        }

        /// <summary>
        /// Try to update before make an Insert of item from repository
        /// </summary>
        /// <param name="item">New or Existing item to insert/Update</param>
        public void InsertOrUpdate<T>(T item) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof(T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));

            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            repo.InsertOrUpdate(item);
        }

        /// <summary>
        /// Update existing item from repository
        /// </summary>
        /// <param name="item">Existing item to update</param>
        public void Update<T>(T item) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof(T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));

            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            repo.Update(item);
        }

        /// <summary>
        /// Remove existing item from repository
        /// </summary>
        /// <param name="item">Old item to delete</param>
        public void Delete<T>(T item) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof(T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));

            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            repo.Delete(item);
        }

        /// <summary>
        /// Persist all changes
        /// </summary>
        public void Commit()
        {
            SqlDataStore.Commit();
        }

        /// <summary>
        /// Undo all changes making after last commit
        /// </summary>
        public void RollBack()
        {
            SqlDataStore.RollBack();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof(T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));

            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            return repo.FirstOrDefault(predicate);
        }

        public IList<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            //Check if we have a repository for this object type.
            var repoType = typeof(T);
            if (!_repositoryList.ContainsKey(repoType)) throw new ArgumentException(string.Format("Unit of works does not contain this type of repository. Type:{0}", repoType));

            //Get repo.
            var repo = (ISqlRepository<T>)_repositoryList[repoType];

            //Transmit request
            return repo.Find(predicate);
        }

        /// <summary>
        /// Indicate if current unit of work is mapped to a Database
        /// </summary>
        public bool IsBinded
        {
            get { return SqlDataStore != null; }
        }
    }
}
