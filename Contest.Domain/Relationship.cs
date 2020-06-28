using System;
using SmartWay.Orm.Attributes;

namespace Contest.Domain
{
    /// <summary>
    ///     Represent a relation n to n between two object
    /// </summary>
    [Entity]
    public class Relationship<TIObj1, TIObj2> : Entity, IRelationship<TIObj1, TIObj2>
        where TIObj1 : class, IEntity
        where TIObj2 : class, IEntity
    {
        public Relationship()
        {
        }

        public Relationship(TIObj1 first, TIObj2 second)
        {
            if (first != null) FirstItemInvolveId = first.Id;
            FirstItemInvolve = first;
            if (second != null) SecondItemInvolveId = second.Id;
            SecondItemInvolve = second;
        }

        /// <summary>
        ///     Get Id of first object involved
        /// </summary>
        [PrimaryKey]
        public Guid FirstItemInvolveId { get; protected set; }

        /// <summary>
        ///     Get first object involved
        /// </summary>
        public TIObj1 FirstItemInvolve { get; set; }

        /// <summary>
        ///     Get Id of second object involved
        /// </summary>
        [PrimaryKey]
        public Guid SecondItemInvolveId { get; protected set; }

        /// <summary>
        ///     Get second object involved
        /// </summary>
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