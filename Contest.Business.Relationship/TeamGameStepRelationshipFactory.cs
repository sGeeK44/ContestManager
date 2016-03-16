using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class TeamGameStepRelationshipFactory : IRelationshipFactory<ITeam, IGameStep>
    {
        public IRelationship<ITeam, IGameStep> Create(ITeam first, IGameStep second)
        {
            return new TeamGameStepRelationship(first, second);
        }
    }
}
