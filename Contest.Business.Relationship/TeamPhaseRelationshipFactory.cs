using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class TeamPhaseRelationshipFactory : IRelationshipFactory<ITeam, IPhase>
    {
        public IRelationship<ITeam, IPhase> Create(ITeam first, IPhase second)
        {
            return new TeamPhaseRelationship(first, second);
        }
    }
}
