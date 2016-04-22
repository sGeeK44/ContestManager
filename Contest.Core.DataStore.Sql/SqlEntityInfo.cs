using System;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlEntityInfo : ISqlEntityInfo
    {
        private Type _classInfo;
        private SqlEntityAttribute _entityAttribute;

        public string TableName { get { return _entityAttribute.Name ?? _classInfo.Name; } }

        private SqlEntityInfo(Type classInfo, SqlEntityAttribute entityAttribute)
        {
            _classInfo = classInfo;
            _entityAttribute = entityAttribute;
        }

        private static SqlEntityInfo Create(Type classInfo, SqlEntityAttribute attr)
        {
            return new SqlEntityInfo(classInfo, attr);
        }

        public static SqlEntityInfo GetEntityInfo<T>()
        {
            var classInfo = typeof(T);
            var entityAttribute = classInfo.GetCustomAttributes(typeof(SqlEntityAttribute), true)
                                           .Cast<SqlEntityAttribute>()
                                           .FirstOrDefault();

            if (entityAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no SqlEntity attribute.", classInfo.Name));

            return Create(classInfo, entityAttribute);
        }
    }
}
