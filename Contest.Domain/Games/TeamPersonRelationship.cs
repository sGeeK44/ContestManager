using Contest.Domain.Players;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    /// <summary>
    ///     Represent a relation between a Team and a GameStep
    /// </summary>
    [Entity(NameInStore = "TEAM_PERSON_RELATION")]
    public class TeamPersonRelationship : Relationship<ITeam, IPerson>
    {
        public TeamPersonRelationship()
        {
            
        }

        /// <summary>
        ///     Instance a new relation
        /// </summary>
        /// <param name="team">Team involve</param>
        /// <param name="person">Person involve</param>
        public TeamPersonRelationship(ITeam team, IPerson person)
            : base(team, person)
        {
        }
    }
}