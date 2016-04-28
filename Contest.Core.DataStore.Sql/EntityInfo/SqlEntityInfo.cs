using System;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlEntityInfo : ISqlEntityInfo
    {
        private readonly Type _classInfo;
        private readonly SqlEntityAttribute _entityAttribute;

        public string TableName { get { return _entityAttribute.GetTableName(_classInfo); } }

        internal SqlEntityInfo(Type classInfo, SqlEntityAttribute entityAttribute)
        {
            _classInfo = classInfo;
            _entityAttribute = entityAttribute;
        }
    }
}
