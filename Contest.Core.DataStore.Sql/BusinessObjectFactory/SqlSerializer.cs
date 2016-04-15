using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Contest.Core.Converters;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Serialization;

namespace Contest.Core.DataStore.Sql.BusinessObjectFactory
{
    public class SqlSerializer<T,TI> : IBusinessObjectFactory<TI>
        where T : class, TI
        where TI : class
    {
        private IConverter Converter { get; set; }

        public SqlSerializer() : this (Converters.Converter.Instance) { }

        public SqlSerializer(IConverter converter)
        {
            Converter = converter;
        }

        public TI Convert(object from)
        {
            return Create(from as IDataReader);
        }

        public TI Create(IDataReader row)
        {
            if (row == null) return default(T);

            //Manage Dynamic & Generic class (Can be recursive)
            var realObjectType = GetRealObjectType(typeof(T), row);

            //Get default constructor
            var constructor = GetDefaultConstructor(realObjectType);
            var result = (TI)constructor.Invoke(null);

            foreach (var propertyInfo in SqlColumnField.GetPropertiesList<T>())
            {
                var fieldAttribute = propertyInfo.GetCustomAttributes(typeof(SqlFieldAttribute), true)
                                                                 .Cast<SqlFieldAttribute>()
                                                                 .FirstOrDefault();
                if (fieldAttribute == null) continue;

                // Converter value
                var sqlValue = row[SqlColumnField.GetColumnName<T>(propertyInfo)];
                if (sqlValue == null) continue;
                
                // Ensure properties have set accessor
                if (!propertyInfo.CanWrite) throw new NotSupportedException(string.Format("You have flags property as DataMember, but setter isn't accessible. Object type:{0}. Property involve: {1} ({2}).", realObjectType.Name, propertyInfo.Name, propertyInfo.PropertyType));

                var innerValue = Converter.Convert(propertyInfo.PropertyType, sqlValue.ToString(), propertyInfo.GetCustomAttributes(true));
                if (innerValue != null) propertyInfo.SetValue(result, innerValue, null); //Set Prop which hold value;
            }
            return result;
        }

        private static ConstructorInfo GetDefaultConstructor(Type objectType)
        {
            var activator = objectType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
            if (activator != null) return activator;
            
            throw new NotSupportedException(string.Format("Class which you want to deserialize doesn't contains default constructor. Class:{0}", typeof(TI).Name));
        }

        public Type GetRealObjectType(Type objectType, IDataReader row)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (row == null) throw new ArgumentNullException("row");

            var result = GetDynamicType(objectType, row);
            return  GetGenericType(result, row);
        }

        private Type GetDynamicType(Type type, IDataReader row)
        {
            Type dynamicEnumPivot;

            if (!type.IsDynamicType(out dynamicEnumPivot)) return type;

            var enumPivotValue = GetEnumPivotValue(dynamicEnumPivot, row);
            var associatedClass = type.GetDynamicType(enumPivotValue, Converter);
            return GetRealObjectType(associatedClass, row);
        }

        private Type GetGenericType(Type type, IDataReader row)
        {
            Type genericEnumPivot;
            if (!type.IsGenericType(out genericEnumPivot)) return type;

            var result = type.GetGenericTypeDefinition();
            var enumPivotValue = GetEnumPivotValue(genericEnumPivot, row);
            var associatedClass = type.GetGenericType(enumPivotValue, Converter);
            var realAssociatedClass = GetRealObjectType(associatedClass, row);
            return result.MakeGenericType(realAssociatedClass);
        }

        public string GetEnumPivotValue(Type enumPivot, IDataReader row)
        {
            PropertyInfo prop;
            try { prop = SqlColumnField.GetPropertiesList<T>().Single(_ => _.PropertyType == enumPivot); }
            catch (InvalidOperationException ex)
            {
                throw new NotSupportedException(string.Format("Enum pivot not or several found. EnumPivot:{0}. CurrentType:{1}", enumPivot, typeof(T)), ex);
            }

            var columnName = SqlColumnField.GetColumnName<T>(prop);
            var sqlValue = row[columnName];
            if (sqlValue == null) throw new NotSupportedException(string.Format("Data reader does not contain column value for enum pivot. EnumPivot:{0}. ColumnName:{1}", enumPivot, columnName));

            return sqlValue.ToString();
        }

        public TI FillOneToManyReferences(TI objToFill, IDataReader row)
        {
            return objToFill;
        }
    }
}
