using System;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public interface IEntityInfoFactory
    {
        ISqlEntityInfo GetEntityInfo<T>();
        ISqlEntityInfo GetEntityInfo(Type type);
    }
}