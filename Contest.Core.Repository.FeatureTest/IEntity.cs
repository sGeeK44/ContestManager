using System;

namespace Contest.Core.Repository.FeatureTest
{
    public interface IEntity : IQueryable
    {
        Guid Key { get; set; }
    }
}