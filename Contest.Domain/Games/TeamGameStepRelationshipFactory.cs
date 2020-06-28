using System.ComponentModel.Composition;
using Contest.Domain.Players;

namespace Contest.Domain.Games
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class TeamGameStepRelationshipFactory : RelationshipFactoryBase<TeamGameStepRelationship, ITeam, IGameStep>
    {
    }
}