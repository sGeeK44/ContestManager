using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using JetBrains.Annotations;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

namespace Contest.Domain.Settings
{
    /// <summary>
    ///     Represent a field where team playing a match.
    /// </summary>
    [Entity(NameInStore = "FIELD")]
    public class Field : Entity, IField
    {
        #region Fields

        private ReferenceHolder<IMatch, Guid> _matchInProgess;
        private ReferenceHolder<IContest, Guid> _currentContest;

        #endregion

        #region MEF Import

        [Import] private IRepository<Games.Contest, IContest> ContestRepository { get; set; }

        [Import] private IRepository<Match, IMatch> MatchRepository { get; set; }

        #endregion

        #region Constructor

        [UsedImplicitly]
        public Field()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchInProgess = new ReferenceHolder<IMatch, Guid>(MatchRepository);
            _currentContest = new ReferenceHolder<IContest, Guid>(ContestRepository);
        }

        public Field(IContest current, string name)
            : this()
        {
            if (current == null) throw new ArgumentException("current");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            FlippingContainer.Instance.ComposeParts(this);

            Name = name;
            CurrentContest = current;
            MatchInProgess = null;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get name of current field
        /// </summary>
        [Field(FieldName = "NAME")]
        public string Name { get; internal set; }

        /// <summary>
        ///     Get boolean to know if current field is allocated to a match
        /// </summary>
        public bool IsAllocated => MatchInProgess != null;

        /// <summary>
        ///     Get current match id in progress on current field if it is allocated, else null.
        /// </summary>
        [Field(FieldName = "MATCH_IN_PROGRESS_ID")]
        public Guid MatchInProgessId
        {
            get => _matchInProgess.Id;
            private set => _matchInProgess.Id = value;
        }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IMatch MatchInProgess
        {
            get => _matchInProgess.Object;
            private set => _matchInProgess.Object = value;
        }

        /// <summary>
        ///     Get current contest id
        /// </summary>
        [Field(FieldName = "CURRENT_CONTEST_ID")]
        public Guid CurrentContestId
        {
            get => _currentContest.Id;
            private set => _currentContest.Id = value;
        }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IContest CurrentContest
        {
            get => _currentContest.Object;
            private set => _currentContest.Object = value;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Allocate current field forspecified match
        /// </summary>
        /// <param name="match">Match for wich you allocated field</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException">Throw when current field is already allocated.</exception>
        public void Allocate(IMatch match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (IsAllocated)
            {
                if (match != MatchInProgess)
                    throw new ArgumentException(
                        $"Le terrain est déjà alloué pour une autre partie. Match en cours:{MatchInProgess.Id}");
                return;
            }

            MatchInProgess = match;
        }

        /// <summary>
        ///     Release current field of allocated match.
        /// </summary>
        public void Release()
        {
            MatchInProgess = null;
        }

        #endregion
    }
}