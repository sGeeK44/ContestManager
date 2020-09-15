using System.Collections.Generic;
using Contest.Domain.Games;

namespace Contest.Domain.Players
{
    public interface IPerson : IEntity
    {
        string LastName { get; set; }

        string FirstName { get; set; }

        string Alias { get; set; }

        string Mail { get; set; }

        bool CanMailing { get; set; }

        bool IsMember { get; set; }

        IList<IRelationship<ITeam, IPerson>> TeamList { get; }

        void SetIndentity(string lastName, string firstName, string alias);

        ITeam GetTeam(IContest contest);
    }
}