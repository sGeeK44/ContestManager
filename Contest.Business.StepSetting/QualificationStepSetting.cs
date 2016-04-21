using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a QualificationStepSetting
    /// </summary>
    [DataContract(Name = "QUALIFICATION_STEP_SETTING")]
    public class QualificationStepSetting : StepSetting, IQualificationStepSetting
    {
        #region Constructors

        private QualificationStepSetting() { }

        /// <summary>
        /// Instance a new QualificationStepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for qualification step</param>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countTeamQualified">Set number team qualified by group when type of phase is qualification, else null</param>
        /// <param name="matchWithRevenche">Set to true if you want two match by team</param>
        public QualificationStepSetting(MatchSetting matchSetting, EliminationType mainEliminationStep, ushort countTeamQualified, bool matchWithRevenche)
            : base(matchSetting)
        {
            MatchWithRevenche = matchWithRevenche;
            SetCountQualifiedTeam(mainEliminationStep, countTeamQualified);
        }

        /// <summary>
        /// Instance a new QualificationStepSetting with specified param
        /// </summary>
        /// <param name="matchSetting">Set match setting for qualification step</param>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countGroup">Set number of qualification group when type of phase is qualification, else null</param>
        /// <param name="matchWithRevenche">Set to true if you want two match by team</param>
        public QualificationStepSetting(MatchSetting matchSetting, ushort countGroup, EliminationType mainEliminationStep, bool matchWithRevenche)
            : base(matchSetting)
        {
            SetCountGroup(countGroup, mainEliminationStep);
            MatchWithRevenche = matchWithRevenche;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get number of group in qualification step
        /// </summary>
        [SqlField(Name = "NUMBER_GROUP")]
        public ushort CountGroup { get; private set; }

        /// <summary>
        /// Get number of qualified team per group
        /// </summary>
        [SqlField(Name = "NUMBER_QUALIFIED_TEAM")]
        public ushort CountTeamQualified { get; private set; }

        /// <summary>
        /// Get number of fished team
        /// </summary>
        [SqlField(Name = "NUMBER_TEAM_FISHED")]
        public ushort CountTeamFished { get; private set; }

        /// <summary>
        /// Indicate if match have revenche
        /// </summary>
        [SqlField(Name = "WITH_REVANCHE")]
        public bool MatchWithRevenche { get; set; }

        /// <summary>
        /// Get minimum team register.
        /// </summary>
        public ushort MinTeamRegister { get { return (ushort)(CountTeamQualified + 1); } }

        #endregion

        #region Methods

        /// <summary>
        /// Set value of CountGroup, CountQualifiedTeam and CountFishedTeam in according with business rules
        /// </summary>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countGroup">Set number of qualification group when type of phase is qualification, else null</param>
        public void SetCountGroup(ushort countGroup, EliminationType mainEliminationStep)
        {
            ushort countTeamQualified, countTeamFished;
            ComputeGroup(countGroup, mainEliminationStep, out countTeamQualified, out countTeamFished);
            CountGroup = countGroup;
            CountTeamQualified = countTeamQualified;
            CountTeamFished = countTeamFished;
        }
        
        /// <summary>
        /// Set value of CountGroup, CountQualifiedTeam and CountFishedTeam in according with business rules
        /// </summary>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countTeamQualified">Set number team qualified by group when type of phase is qualification, else null</param>
        public void SetCountQualifiedTeam(EliminationType mainEliminationStep, ushort countTeamQualified)
        {
            ushort countGroup, countTeamFished;
            ComputeGroup(mainEliminationStep, countTeamQualified, out countGroup, out countTeamFished);
            CountTeamQualified = countTeamQualified;
            CountGroup = countGroup;
            CountTeamFished = countTeamFished;
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public override void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            base.PrepareCommit(unitOfWorks);
            unitOfWorks.InsertOrUpdate<IQualificationStepSetting>(this);
            MatchSetting.PrepareCommit(unitOfWorks);
        }

        #endregion

        #region Static methods

        public static void ComputeGroup(EliminationType eliminationStep, ushort countTeamQualified, out ushort countGroup, out ushort countFishedTeam)
        {
            countGroup = (ushort)((ushort)eliminationStep * 2 / countTeamQualified);
            countFishedTeam = (ushort)((ushort)eliminationStep * 2 % countTeamQualified);
            
        }

        public static void ComputeGroup(ushort countGroup, EliminationType eliminationStep, out ushort countTeamQualified, out ushort countFishedTeam)
        {
            countTeamQualified = (ushort)((ushort)eliminationStep * 2 / countGroup);
            countFishedTeam = (ushort)((ushort)eliminationStep * 2 % countGroup);
        }

        #endregion
    }
}
