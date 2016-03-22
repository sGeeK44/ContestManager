using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class TeamPhaseRelationshipFactory : RelationshipFactoryBase<TeamPhaseRelationship, ITeam, IPhase> { }
}
