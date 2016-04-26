using System;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [SqlEntity(Name = "ELIMINATION_STEP_SETTING")]
    public class EliminationStepSetting : StepSetting, IEliminationStepSetting
    {
        #region Constructors

        private EliminationStepSetting() {}

        /// <summary>
        /// Instance a new EliminationStepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for qualification step</param>
        /// <param name="firstStep">Set the first step for elimination step</param>
        public EliminationStepSetting(IMatchSetting matchSetting, EliminationType firstStep)
            : base(matchSetting)
        {
            FirstStep = firstStep;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get first elimination step
        /// </summary>
        [SqlField(Name = "FIRST_STEP")]
        public EliminationType FirstStep { get; set; }

        #endregion

        #region Methods

        public override void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            base.PrepareCommit(unitOfWorks);
            unitOfWorks.InsertOrUpdate<IEliminationStepSetting>(this);
        }

        public override void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotFiniteNumberException();
        }

        #endregion
    }
}
