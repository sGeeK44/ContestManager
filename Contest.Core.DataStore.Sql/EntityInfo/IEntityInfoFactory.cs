using System;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public interface IEntityInfoFactory
    {
        ISqlEntityInfo GetEntityInfo<T>();
        ISqlEntityInfo GetEntityInfo(Type type);
    }
}