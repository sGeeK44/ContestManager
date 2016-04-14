using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Contest.Core.Converters;
using Contest.Core.Converters.EnumConverter;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Serialization;

namespace Contest.Core.DataStore.Sql.BusinessObjectFactory
{
    public class SqlSerializer<T,TI> : IBusinessObjectFactory<TI>
        where T : class, TI
        where TI : class
    {
        private IConverter Instance { get; set; }
        private IEnumConverter PivotEnumConverter { get; set; }

        public SqlSerializer()
        {
            Instance = Converter.Instance;
            PivotEnumConverter = new StringEnumByInt();
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
            var activator = realObjectType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
            if (activator == null) throw new Exception(string.Format("Class which you want to deserialize doesn't contains default constructor. Class:{0}", typeof(TI).Name));
            var result = (TI)activator.Invoke(null);
            foreach (var propertyInfo in SqlColumnField.GetPropertiesList<T>())
            {
                var fieldAttribute = propertyInfo.GetCustomAttributes(typeof(SqlFieldAttribute), true)
                                                                 .Cast<SqlFieldAttribute>()
                                                                 .FirstOrDefault();
                if (fieldAttribute == null) continue;

                // Ensure properties have set accessor
                if (!propertyInfo.CanWrite) throw new NotSupportedException(string.Format("You have flags property as DataMember, but setter isn't accessible. Object type:{0}. Property involve: {1} ({2}).", realObjectType.Name, propertyInfo.Name, propertyInfo.PropertyType));

                // Converter value
                var innerValue = Instance.Convert(propertyInfo.PropertyType, row[fieldAttribute.Name ?? propertyInfo.Name].ToString(), propertyInfo.GetCustomAttributes(true));
                if (innerValue == null) continue; //If inner value is null don't set property
                propertyInfo.SetValue(result, innerValue, null); //Set Prop which hold value;
            }
            return result;
        }

        private Type GetRealObjectType(Type objectType, IDataReader row)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");
            if (row == null) throw new ArgumentNullException("row");
            Type dynamicEnumPivot;
            var isDynamicType = objectType.IsDynamicType(out dynamicEnumPivot);

            var result = isDynamicType
                       ? GetRealObjectType(objectType.GetDynamicType(GetStringEnumPivotValue(dynamicEnumPivot, row), PivotEnumConverter), row)
                       : objectType;

            //Check if object is generic
            Type genericEnumPivot;
            if (!result.IsGenericType(out genericEnumPivot)) return result;

            //Get generic type definition
            result = result.GetGenericTypeDefinition();
            result = result.MakeGenericType(GetRealObjectType(result.GetGenericType(GetStringEnumPivotValue(genericEnumPivot, row), PivotEnumConverter), row)); //Set generic type
            return result;
        }

        private string GetStringEnumPivotValue(Type enumPivot, IDataReader row)
        {
            //Iterate all potential properties to find same type as enumPivot
            foreach (var prop in SqlColumnField.GetPropertiesList<T>())
            {
                // If we found a property with same type as enum pivot
                if (prop.PropertyType != enumPivot) continue;
                var fieldAttribute = prop.GetCustomAttributes(typeof(SqlFieldAttribute), true)
                                                         .Cast<SqlFieldAttribute>()
                                                         .FirstOrDefault();
                if (fieldAttribute == null) continue;
                return row[fieldAttribute.Name ?? prop.Name].ToString();
            }

            throw new NotSupportedException(string.Format("Enum pivot not found. EnumPivot:{0}. CurrentType:{1}", enumPivot, typeof(TI)));
        }
    }
}
