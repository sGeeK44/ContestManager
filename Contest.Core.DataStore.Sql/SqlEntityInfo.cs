using System;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlEntityInfo : ISqlEntityInfo
    {
        private readonly Type _classInfo;
        private readonly SqlEntityAttribute _entityAttribute;

        public string TableName { get { return _entityAttribute.GetTableName(_classInfo); } }

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
            return GetEntityInfo(typeof(T));
        }

        public static SqlEntityInfo GetEntityInfo(Type classInfo)
        {
            if (classInfo == null) throw new ArgumentNullException("classInfo");

            var entityAttribute = classInfo.GetCustomAttributes(typeof(SqlEntityAttribute), true)
                                           .Cast<SqlEntityAttribute>()
                                           .FirstOrDefault();

            if (entityAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no SqlEntity attribute.", classInfo.Name));

            return Create(classInfo, entityAttribute);
        }
    }
}
