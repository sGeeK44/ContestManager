using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql
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
        [DataMember]
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
            var castedOther = other as IIdentifiable;
            if (castedOther == null) return false;

            return Id == castedOther.Id;
        }
    }
}