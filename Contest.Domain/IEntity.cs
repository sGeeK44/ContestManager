using System;

namespace Contest.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}