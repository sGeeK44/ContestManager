using System;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.ReferenceManyToMany
{
    /// <summary>
    /// Represent a relation n to n between two object
    /// </summary>
    [SqlRelationEntity]
    public class Relationship<TIObj1, TIObj2> : IRelationship<TIObj1, TIObj2>
        where TIObj1 : class, IIdentifiable
        where TIObj2 : class, IIdentifiable
    {
        private Relationship() { }

        public Relationship(TIObj1 first, TIObj2 second)
        {
            if (first != null) FirstItemInvolveId = first.Id;
            FirstItemInvolve = first;
            if (second != null) SecondItemInvolveId = second.Id;
            SecondItemInvolve = second;
        }

        /// <summary>
        /// Get Id of first object involved
        /// </summary>
        [SqlPrimaryKey]
        [SqlForeignKey("FirstItemInvolve")]
        public Guid FirstItemInvolveId { get; protected set; }

        /// <summary>
        /// Get first object involved
        /// </summary>
        [SqlManyToOneReference]
        public TIObj1 FirstItemInvolve { get; set; }

        /// <summary>
        /// Get Id of second object involved
        /// </summary>
        [SqlPrimaryKey]
        [SqlForeignKey("SecondItemInvolve")]
        public Guid SecondItemInvolveId { get; protected set; }

        /// <summary>
        /// Get second object involved
        /// </summary>
        [SqlManyToOneReference]
        public TIObj2 SecondItemInvolve { get; set; }

        public bool AreSame(object other)
        {
            return AreSame(other as Relationship<TIObj1, TIObj2>);
        }

        public bool AreSame(Relationship<TIObj1, TIObj2> other)
        {
            if (other == null) return false;

            return FirstItemInvolveId == other.FirstItemInvolveId
                && SecondItemInvolveId == other.SecondItemInvolveId;
        }
    }
}
