using System;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a relation between a Team and a GameStep
    /// </summary>
    [DataContract(Name = "TEAM_GAME_STEP_RELATION")]
    public class TeamGameStepRelationship : Relationship<ITeam, IGameStep>, ISqlPersistable
    {
        public TeamGameStepRelationship(IUnitOfWorks unitOfWorks)
            : base(unitOfWorks)
        { }

        /// <summary>
        /// Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="gameStep">Game step involve</param>
        public TeamGameStepRelationship(ITeam team, IGameStep gameStep)
            : base(team, gameStep) { }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IRelationship<ITeam, IGameStep>>(this);
        }
        
        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }
    }
}
