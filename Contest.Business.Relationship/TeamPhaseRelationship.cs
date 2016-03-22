using System;
using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a relation between a Team and a Phase
    /// </summary>
    [DataContract(Name = "TEAM_PHASE_RELATION")]
    public class TeamPhaseRelationship : Relationship<Team, ITeam, Phase, IPhase>, IRelationship<ITeam, IPhase>
    {
        public TeamPhaseRelationship() { }

        /// <summary>
        /// Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="phase">Game step involve</param>
        public TeamPhaseRelationship(ITeam team, IPhase phase)
            : base(team, phase) { }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IRelationship<ITeam, IPhase>>(this);
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
