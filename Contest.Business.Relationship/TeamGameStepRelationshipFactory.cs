using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class TeamGameStepRelationshipFactory : RelationshipFactoryBase<TeamGameStepRelationship, ITeam, IGameStep> { }
}
