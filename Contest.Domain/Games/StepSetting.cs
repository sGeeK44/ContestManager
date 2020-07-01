using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Matchs;
using JetBrains.Annotations;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    /// <summary>
    ///     Represent a StepSetting
    /// </summary>
    public abstract class StepSetting : Entity, IStepSetting
    {
        #region Fields

        private Lazy<IMatchSetting> _matchSetting;

        #endregion

        #region MEF Import

        [Import] private IRepository<MatchSetting, IMatchSetting> MatchSettingRepository { get; set; }

        #endregion

        #region Constructors

        [UsedImplicitly]
        protected StepSetting()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchSetting = new Lazy<IMatchSetting>(() =>
                MatchSettingRepository.FirstOrDefault(_ => _.Id == MatchSettingId));
        }

        /// <summary>
        ///     Instance a new StepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for  step</param>
        protected StepSetting(IMatchSetting matchSetting)
        {
            MatchSetting = matchSetting ?? throw new ArgumentNullException(nameof(matchSetting));
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        [Field(FieldName = "MATCH_SETTING_ID")]
        public Guid MatchSettingId { get; private set; }

        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        public IMatchSetting MatchSetting
        {
            get => _matchSetting.Value;
            private set
            {
                _matchSetting = new Lazy<IMatchSetting>(() => value);
                MatchSettingId = value?.Id ?? Guid.Empty;
            }
        }

        #endregion
    }
}