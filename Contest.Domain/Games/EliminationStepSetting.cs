using Contest.Domain.Matchs;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    [Entity(NameInStore = "ELIMINATION_STEP_SETTING")]
    public class EliminationStepSetting : StepSetting, IEliminationStepSetting
    {
        #region Properties

        /// <summary>
        ///     Get first elimination step
        /// </summary>
        [Field(FieldName = "FIRST_STEP")]
        public EliminationType FirstStep { get; set; }

        #endregion

        #region Constructors

        private EliminationStepSetting()
        {
        }

        /// <summary>
        ///     Instance a new EliminationStepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for qualification step</param>
        /// <param name="firstStep">Set the first step for elimination step</param>
        public EliminationStepSetting(IMatchSetting matchSetting, EliminationType firstStep)
            : base(matchSetting)
        {
            FirstStep = firstStep;
        }

        #endregion
    }
}