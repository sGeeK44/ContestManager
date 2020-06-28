using System.ComponentModel.Composition;
using Contest.Domain.Players;

namespace Contest.Domain.Games
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class TeamPhaseRelationshipFactory : RelationshipFactoryBase<TeamPhaseRelationship, ITeam, IPhase>
    {
    }
}