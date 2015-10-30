using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a relation between a Team and a GameStep
    /// </summary>
    [DataContract(Name = "TEAM_GAME_STEP_RELATION")]
    public class TeamGameStepRelationship : Relationship<ITeam, IGameStep>
    {
        protected TeamGameStepRelationship()
        { }

        /// <summary>
        /// Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="gameStep">Game step involve</param>
        public TeamGameStepRelationship(ITeam team, IGameStep gameStep)
            : base(team, gameStep) { }

        public void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate(this);
        }
    }
}
