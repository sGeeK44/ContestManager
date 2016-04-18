using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a StepSetting
    /// </summary>
    public abstract class StepSetting : Identifiable<StepSetting>, IStepSetting
    {
        #region Fields

        private Lazy<IMatchSetting> _matchSetting;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IMatchSetting> MatchSettingRepository { get; set; }

        #endregion

        #region Constructors

        protected StepSetting()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _matchSetting = new Lazy<IMatchSetting>(() => MatchSettingRepository.FirstOrDefault(_ => _.Id == MatchSettingId));
        }

        /// <summary>
        /// Instance a new StepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for  step</param>
        protected StepSetting(IMatchSetting matchSetting)
        {
            if (matchSetting == null) throw new ArgumentNullException("matchSetting");

            MatchSetting = matchSetting;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get match's setting for  step
        /// </summary>
        [DataMember(Name = "MATCH_SETTING_ID")]
        public Guid MatchSettingId { get; private set; }

        /// <summary>
        /// Get match's setting for  step
        /// </summary>
        public IMatchSetting MatchSetting
        {
            get { return _matchSetting.Value; }
            private set
            {
                _matchSetting = new Lazy<IMatchSetting>(() => value);
                MatchSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        #endregion

        #region Methods

        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            MatchSetting.PrepareCommit(unitOfWorks);
        }

        public virtual void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.Delete(this);
            MatchSetting.PrepareDelete(unitOfWorks);
        }

        #endregion
    }
}
