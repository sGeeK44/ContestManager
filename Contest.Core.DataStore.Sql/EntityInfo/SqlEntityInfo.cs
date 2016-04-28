using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlEntityInfo : ISqlEntityInfo
    {
        private readonly Type _classInfo;
        private readonly SqlEntityAttribute _entityAttribute;

        private IEntityInfoFactory EntityInfoFactory { get; set; }

        public string TableName { get { return _entityAttribute.GetTableName(EntityInfoFactory, _classInfo); } }

        public IList<ISqlPropertyInfo> FieldList { get; set; }

        internal SqlEntityInfo(IEntityInfoFactory entityInfoFactory, Type classInfo, SqlEntityAttribute entityAttribute)
        {
            EntityInfoFactory = entityInfoFactory;

            _classInfo = classInfo;
            _entityAttribute = entityAttribute;
        }
    }
}
