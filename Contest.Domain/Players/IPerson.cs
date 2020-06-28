using System;

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

        Guid AffectedTeamId { get; set; }

        ITeam AffectedTeam { get; set; }

        void SetIndentity(string lastName, string firstName, string alias);
    }
}