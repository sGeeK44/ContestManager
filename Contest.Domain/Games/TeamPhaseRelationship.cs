using Contest.Domain.Players;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    /// <summary>
    ///     Represent a relation between a Team and a Phase
    /// </summary>
    [Entity(NameInStore = "TEAM_PHASE_RELATION")]
    public class TeamPhaseRelationship : Relationship<ITeam, IPhase>
    {
        public TeamPhaseRelationship()
        {
            
        }

        /// <summary>
        ///     Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="phase">Game step involve</param>
        public TeamPhaseRelationship(ITeam team, IPhase phase)
            : base(team, phase)
        {
        }
    }
}