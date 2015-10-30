﻿using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a field where team playing a match.
    /// </summary>
    [DataContract(Name = "FIELD")]
    public class Field : Identifiable<Field>, IField
    {
        #region Fields

        private Lazy<IMatch> _matchInProgess;
        private Lazy<IContest> _currentContest;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IContest> ContestRepository { get; set; }

        [Import]
        private IRepository<IMatch> MatchRepository { get; set; }

        #endregion

        #region Constructor

        private Field()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchInProgess = new Lazy<IMatch>(() => MatchRepository.FirstOrDefault(_ => _.Id == MatchInProgessId));
            _currentContest = new Lazy<IContest>(() => ContestRepository.FirstOrDefault(_ => _.Id == CurrentContestId));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get name of current field
        /// </summary>
        [DataMember(Name = "NAME")]
        public string Name { get; private set; }

        /// <summary>
        /// Get boolean to know if current field is allocated to a match
        /// </summary>
        public bool IsAllocated { get { return MatchInProgess != null; } }

        /// <summary>
        /// Get current match id in progress on current field if it is allocated, else null.
        /// </summary>
        [DataMember(Name = "MATCH_IN_PROGRESS_ID")]
        public Guid MatchInProgessId { get; private set; }

        /// <summary>
        /// Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IMatch MatchInProgess
        {
            get { return _matchInProgess.Value; }
            private set
            {
                _matchInProgess = new Lazy<IMatch>(() => value);
                MatchInProgessId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get current contest id
        /// </summary>
        [DataMember(Name = "CURRENT_CONTEST_ID")]
        public Guid CurrentContestId { get; private set; }

        /// <summary>
        /// Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IContest CurrentContest
        {
            get { return _currentContest.Value; }
            private set
            {
                _currentContest = new Lazy<IContest>(() => value);
                CurrentContestId = value != null ? value.Id : Guid.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Allocate current field forspecified match
        /// </summary>
        /// <param name="match">Match for wich you allocated field</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException">Throw when current field is already allocated.</exception>
        public void Allocate(IMatch match)
        {
            if (match == null) throw new ArgumentNullException("match");
            if (IsAllocated) throw new ArgumentException(string.Format("Le terrain est déjà alloué pour une autre partie. Match en cours:{0}", MatchInProgess.Id));
            
            MatchInProgess = match;
        }

        /// <summary>
        /// Release current field of allocated match.
        /// </summary>
        public void Release()
        {
            MatchInProgess = null;
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IField>(this);
        }

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance of Field with specified param
        /// </summary>
        /// <param name="current">Current contest</param>
        /// <param name="name">Name of new field</param>
        /// <returns>Field's instance</returns>
        public static IField Create(Contest current, string name)
        {
            if (current == null) throw new ArgumentException("current");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new Field
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    CurrentContest = current
                };
        }

        #endregion
    }
}
