using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Matchs;
using JetBrains.Annotations;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

namespace Contest.Domain.Games
{
    /// <summary>
    ///     Represent a StepSetting
    /// </summary>
    public abstract class StepSetting : Entity, IStepSetting
    {
        #region Fields

        private ReferenceHolder<IMatchSetting, Guid> _matchSetting;

        #endregion

        #region MEF Import

        [Import] private IRepository<MatchSetting, IMatchSetting> MatchSettingRepository { get; set; }

        #endregion

        #region Constructors

        [UsedImplicitly]
        public StepSetting()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchSetting = new ReferenceHolder<IMatchSetting, Guid>(MatchSettingRepository);
        }

        /// <summary>
        ///     Instance a new StepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for  step</param>
        protected StepSetting(IMatchSetting matchSetting)
            : this()
        {
            MatchSetting = matchSetting ?? throw new ArgumentNullException(nameof(matchSetting));
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        [Field(FieldName = "MATCH_SETTING_ID")]
        public Guid MatchSettingId
        {
            get => _matchSetting.Id;
            private set => _matchSetting.Id= value;
        }

        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        public IMatchSetting MatchSetting
        {
            get => _matchSetting.Object;
            private set => _matchSetting.Object = value;
        }

        #endregion
    }
}