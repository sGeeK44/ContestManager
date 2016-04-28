using System;
using Contest.Core.DataStore.Sql.SqlQuery;

namespace Contest.Core.DataStore.Sql.Attributes
{
    internal class SqlRelationEntityAttribute : SqlEntityAttribute
    {
        internal override string GetTableName(IEntityInfoFactory entityInfoFactory, Type classInfo)
        {
            var genericTypes = classInfo.GetGenericArguments();
            var firstEntity = entityInfoFactory.GetEntityInfo(genericTypes[0]);
            var secondEntity = entityInfoFactory.GetEntityInfo(genericTypes[1]);
            return string.Format("R_{0}_{1}", firstEntity.TableName, secondEntity.TableName);
        }
    }
}
