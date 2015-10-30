namespace Contest.Business
{
    public interface IQualificationStepSetting : IStepSetting
    {
        /// <summary>
        /// Get number of group in qualification step
        /// </summary>
        ushort CountGroup { get; }

        /// <summary>
        /// Get number of qualified team per group
        /// </summary>
        ushort CountTeamQualified { get; }

        /// <summary>
        /// Get number of fished team
        /// </summary>
        ushort CountTeamFished { get; }

        /// <summary>
        /// Indicate if match have revenche
        /// </summary>
        bool MatchWithRevenche { get; set; }

        /// <summary>
        /// Get minimum team register.
        /// </summary>
        ushort MinTeamRegister { get; }

        /// <summary>
        /// Get match's setting for  step
        /// </summary>
        IMatchSetting MatchSetting { get; }

        /// <summary>
        /// Set value of CountGroup, CountQualifiedTeam and CountFishedTeam in according with business rules
        /// </summary>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countGroup">Set number of qualification group when type of phase is qualification, else null</param>
        void SetCountGroup(ushort countGroup, EliminationType mainEliminationStep);

        /// <summary>
        /// Set value of CountGroup, CountQualifiedTeam and CountFishedTeam in according with business rules
        /// </summary>
        /// <param name="mainEliminationStep">First eliminiation step for main phase</param>
        /// <param name="countTeamQualified">Set number team qualified by group when type of phase is qualification, else null</param>
        void SetCountQualifiedTeam(EliminationType mainEliminationStep, ushort countTeamQualified);
    }
}