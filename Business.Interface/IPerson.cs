using System;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IPerson : IIdentifiable, IQueryable, ISqlPersistable
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