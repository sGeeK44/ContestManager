using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;
using SmartWay.Orm.Repositories;

namespace Contest.Domain
{
    /// <summary>
    ///     Represent a relation n to n between two object
    /// </summary>
    public class Relationship<TIObj1, TIObj2> : Entity, IRelationship<TIObj1, TIObj2>
        where TIObj1 : class, IEntity
        where TIObj2 : class, IEntity
    {
        private const string UniqueRelation = "UniqueRelation";
        private ReferenceHolder<TIObj1, Guid> _firstItem;
        private ReferenceHolder<TIObj2, Guid> _secondItem;

        [Import] private IRepository<TIObj1> FirstItemRepository { get; set; }

        [Import] private IRepository<TIObj2> SecondItemRepository { get; set; }

        public Relationship()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _firstItem = new ReferenceHolder<TIObj1, Guid>(FirstItemRepository);
            _secondItem = new ReferenceHolder<TIObj2, Guid>(SecondItemRepository);
        }

        public Relationship(TIObj1 first, TIObj2 second)
            : this()
        {
            if (first != null) FirstItemInvolveId = first.Id;
            FirstItemInvolve = first;
            if (second != null) SecondItemInvolveId = second.Id;
            SecondItemInvolve = second;
        }

        /// <summary>
        ///     Get Id of first object involved
        /// </summary>
        [Field(Indexes = new []{ UniqueRelation })]
        public Guid FirstItemInvolveId
        {
            get => _firstItem.Id;
            private set => _firstItem.Id = value;
        }

        /// <summary>
        ///     Get first object involved
        /// </summary>
        public TIObj1 FirstItemInvolve
        {
            get => _firstItem.Object;
            set => _firstItem.Object = value;
        }

        /// <summary>
        ///     Get Id of second object involved
        /// </summary>
        [Field(Indexes = new []{ UniqueRelation })]
        public Guid SecondItemInvolveId
        {
            get => _secondItem.Id;
            private set => _secondItem.Id = value;
        }

        /// <summary>
        ///     Get second object involved
        /// </summary>
        public TIObj2 SecondItemInvolve
        {
            get => _secondItem.Object;
            set => _secondItem.Object = value;
        }

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