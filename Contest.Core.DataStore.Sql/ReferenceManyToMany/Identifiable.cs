using System;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.ReferenceManyToMany
{
    /// <summary>
    /// Represent a class with a unique Guid identifier
    /// </summary>
    [Serializable]
    public class Identifiable<T> : IIdentifiable, IQueryable
    {
        private Guid _id;

        /// <summary>
        /// Create a new instance of Identifiable class with new Guid.
        /// </summary>
        public Identifiable()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Get the unique Guid Identifier for current instance
        /// </summary>
        [SqlPrimaryKey]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        /// <summary>
        /// Determine if specified object is same object than current
        /// </summary>
        /// <returns>True if they are identic, false else</returns>
        public bool AreSame(object other)
        {
            var castedOther = other as Identifiable<T>;
            if (castedOther == null) return false;

            return Id == castedOther.Id;
        }
    }
}