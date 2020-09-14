using System;
using SmartWay.Orm.Entity;

namespace Contest.Domain
{
    public interface IEntity : IDistinctableEntity
    {
        Guid Id { get; }
    }
}