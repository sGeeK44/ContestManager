using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using JetBrains.Annotations;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Settings
{
    /// <summary>
    ///     Represent a field where team playing a match.
    /// </summary>
    [Entity(NameInStore = "FIELD")]
    public class Field : Entity, IField
    {
        #region Fields

        private Lazy<IMatch> _matchInProgess;
        private Lazy<IContest> _currentContest;

        #endregion

        #region MEF Import

        [Import] private IRepository<IContest> ContestRepository { get; set; }

        [Import] private IRepository<IMatch> MatchRepository { get; set; }

        #endregion

        #region Constructor

        [UsedImplicitly]
        public Field()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchInProgess =
                new Lazy<IMatch>(() => MatchRepository.FirstOrDefault(_ => _.Id == MatchInProgessId));
            _currentContest = new Lazy<IContest>(() =>
                ContestRepository.FirstOrDefault(_ => _.Id == CurrentContestId));
        }

        public Field(IContest current, string name)
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
        public Guid MatchInProgessId { get; private set; }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IMatch MatchInProgess
        {
            get => _matchInProgess.Value;
            private set
            {
                _matchInProgess = new Lazy<IMatch>(() => value);
                MatchInProgessId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get current contest id
        /// </summary>
        [Field(FieldName = "CURRENT_CONTEST_ID")]
        public Guid CurrentContestId { get; private set; }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        public IContest CurrentContest
        {
            get => _currentContest.Value;
            internal set
            {
                _currentContest = new Lazy<IContest>(() => value);
                CurrentContestId = value?.Id ?? Guid.Empty;
            }
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