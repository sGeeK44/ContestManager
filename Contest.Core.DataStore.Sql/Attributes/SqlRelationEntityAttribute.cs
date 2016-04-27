﻿using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    internal class SqlRelationEntityAttribute : SqlEntityAttribute
    {
        internal override string GetTableName(Type classInfo)
        {
            var genericTypes = classInfo.GetGenericArguments();
            var firstEntity = SqlEntityInfo.GetEntityInfo(genericTypes[0]);
            var secondEntity = SqlEntityInfo.GetEntityInfo(genericTypes[1]);
            return string.Format("R_{0}_{1}", firstEntity.TableName, secondEntity.TableName);
        }
    }
}
