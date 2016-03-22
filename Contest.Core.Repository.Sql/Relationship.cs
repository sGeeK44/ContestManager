﻿using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using Contest.Core.Component;

namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Represent a relation n to n between two object
    /// </summary>
    public class Relationship<TObj1, TIObj1, TObj2, TIObj2>
        where TObj1 : class, TIObj1
        where TObj2 : class, TIObj2
        where TIObj1 : class, IIdentifiable
        where TIObj2 : class, IIdentifiable
    {
        private Lazy<TIObj1> _firstItemInvolve;
        private Lazy<TIObj2> _secondItemInvolve;

        [Import]
        internal IRepository<TIObj1> FirstItemRepository { get; set; }

        [Import]
        internal IRepository<TIObj2> SecondItemRepository { get; set; }

        public Relationship()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _firstItemInvolve = new Lazy<TIObj1>(() => FirstItemRepository.FirstOrDefault(_ => _.Id == FirstItemInvolveId));
            _secondItemInvolve = new Lazy<TIObj2>(() => SecondItemRepository.FirstOrDefault(_ => _.Id == SecondItemInvolveId));
        }

        public Relationship(TIObj1 first, TIObj2 second)
        {
            FlippingContainer.Instance.ComposeParts(this);
            FirstItemInvolve = first;
            SecondItemInvolve = second;
        }

        /// <summary>
        /// Get Id of first object involved
        /// </summary>
        [DataMember]
        [SqlPrimaryKey]
        public Guid FirstItemInvolveId { get; protected set; }

        /// <summary>
        /// Get first object involved
        /// </summary>
        public TIObj1 FirstItemInvolve
        {
            get { return _firstItemInvolve.Value; }
            set
            {
                _firstItemInvolve = new Lazy<TIObj1>(() => value);
                FirstItemInvolveId = value != null ? value.Id : default(Guid);
            }
        }

        /// <summary>
        /// Get Id of second object involved
        /// </summary>
        [DataMember]
        [SqlPrimaryKey]
        public Guid SecondItemInvolveId { get; protected set; }

        /// <summary>
        /// Get second object involved
        /// </summary>
        public TIObj2 SecondItemInvolve
        {
            get { return _secondItemInvolve.Value; }
            set
            {
                _secondItemInvolve = new Lazy<TIObj2>(() => value);
                SecondItemInvolveId = value != null ? value.Id : default(Guid);
            }
        }

        public bool AreSame(object other)
        {
            var castedObject = other as Relationship<TObj1, TIObj1, TObj2, TIObj2>;
            if (castedObject == null) return false;

            return FirstItemInvolve.Id == castedObject.FirstItemInvolve.Id
                && SecondItemInvolve.Id == castedObject.SecondItemInvolve.Id;
        }
    }
}
