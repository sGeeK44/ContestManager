using Contest.Domain.Players;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    /// <summary>
    ///     Represent a relation between a Team and a GameStep
    /// </summary>
    [Entity(NameInStore = "TEAM_GAME_STEP_RELATION")]
    public class TeamGameStepRelationship : Relationship<ITeam, IGameStep>
    {
        public TeamGameStepRelationship()
        {
            
        }

        /// <summary>
        ///     Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="gameStep">Game step involve</param>
        public TeamGameStepRelationship(ITeam team, IGameStep gameStep)
            : base(team, gameStep)
        {
        }
    }
}