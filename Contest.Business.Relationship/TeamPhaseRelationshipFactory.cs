using System.ComponentModel.Composition;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class TeamPhaseRelationshipFactory : RelationshipFactoryBase<TeamPhaseRelationship, ITeam, IPhase> { }
}
