using System.ComponentModel.Composition;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Business
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class TeamGameStepRelationshipFactory : RelationshipFactoryBase<TeamGameStepRelationship, ITeam, IGameStep> { }
}
