using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Contest.Core.Converters;
using Contest.Core.Converters.EnumConverter;
using Contest.Core.Serialization;

namespace Contest.Core.Repository.Sql
{
    public class SqlBuilder<T,TI> : ISqlBuilder<T, TI> where T : class, TI
        where TI : class
    {
        private IConverter Instance { get; set; }
        private IEnumConverter PivotEnumConverter { get; set; }

        public SqlBuilder()
        {
            Instance = Converter.Instance;
            PivotEnumConverter = new StringEnumByInt();
        }

        public TI CreateInstance(IDataReader row)
        {
            if (row == null) return default(T);

            //Manage Dynamic & Generic class (Can be recursive)
            var realObjectType = GetRealObjectType(typeof(T), row);

            //Get default constructor
            var activator = realObjectType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
            if (activator == null) throw new Exception(string.Format("Class which you want to deserialize doesn't contains default constructor. Class:{0}", typeof(TI).Name));
            var result = (TI)activator.Invoke(null);
            foreach (var propertyInfo in GetPropertiesList(true))
            {
                var fieldAttribute = propertyInfo.GetCustomAttributes(typeof (DataMemberAttribute), true)
                                                                 .Cast<DataMemberAttribute>()
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
            var result = objectType.IsDynamicType(out dynamicEnumPivot) //If type is Dynamique
                        ? GetRealObjectType(objectType.GetDynamicType(GetStringEnumPivotValue(dynamicEnumPivot, row), PivotEnumConverter), row) //Return RealType resursivly of DynamiqueType founded
                        : objectType; //Else keep type

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
            foreach (var prop in GetPropertiesList(true))
            {
                // If we found a property with same type as enum pivot
                if (prop.PropertyType != enumPivot) continue;
                var fieldAttribute = prop.GetCustomAttributes(typeof(DataMemberAttribute), true)
                                                         .Cast<DataMemberAttribute>()
                                                         .FirstOrDefault();
                if (fieldAttribute == null) continue;
                return row[fieldAttribute.Name ?? prop.Name].ToString();
            }
            throw new NotSupportedException(string.Format("Enum pivot not found. EnumPivot:{0}. CurrentType:{1}", enumPivot, typeof(TI)));
        }

        public string CreateTable()
        {
            var tableName  = GetTableName();
            var fieldList = (from propertyInfo in GetPropertiesList(true)
                                      let fieldAttribute = propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), true)
                                                                       .Cast<DataMemberAttribute>()
                                                                       .FirstOrDefault()
                                      where fieldAttribute != null
                                      select string.Format("{0} {1}{2}",
                                                           fieldAttribute.Name ?? propertyInfo.Name,
                                                           ToSqlType(propertyInfo.PropertyType),
                                                           propertyInfo.GetCustomAttributes(typeof(SqlPrimaryKeyAttribute), true)
                                                                       .Cast<SqlPrimaryKeyAttribute>()
                                                                       .FirstOrDefault() != null ? " primary key" : string.Empty)).ToList();


            if (fieldList.Count == 0) throw new NotSupportedException("Table have to own one field at less.");

            return string.Format(@"CREATE TABLE IF NOT EXISTS {0} ({1});", tableName, fieldList.Aggregate((x, y) => string.Concat(x, ", ", y)));
        }

        public string Select(Expression<Func<TI, bool>> predicate, out IList<Tuple<string, object, object[]>> arg)
        {
            var tableName = GetTableName();
            StringBuilder fields = null;

            IList<Tuple<string, object, object[]>> dummy;
            foreach (var value in GetValues(null, out dummy))
            {
                //Add marker to set clause
                if (fields == null) fields = new StringBuilder(value.Item1);
                else fields.Append(", " + value.Item1);
            }

            var whereSqlExpression = new WhereSqlExpression<T, TI>(predicate);
            var where = whereSqlExpression.ToStatement(out arg);
            return string.Format(@"SELECT {0} FROM {1}{2};", fields, tableName, string.IsNullOrEmpty(where) ? string.Empty : " " + where);
        }

        public string Insert(TI item, out IList<Tuple<string, object, object[]>> arg)
        {
            var tableName = GetTableName();
            StringBuilder names = null;
            StringBuilder values = null;
            foreach (var value in GetValues(item, out arg))
            {
                //Add field name
                if (names == null) names = new StringBuilder(value.Item1);
                else names.Append(", " + value.Item1);

                // Add marker to values.
                if (values == null) values = new StringBuilder(value.Item2);
                else values.Append(", " + value.Item2);
            }
            return string.Format(@"INSERT INTO {0} ({1}) VALUES ({2});", tableName, names, values);
        }

        public string Update(TI item, out IList<Tuple<string, object, object[]>> arg)
        {
            var tableName = GetTableName();
            StringBuilder values = null;
            StringBuilder keys = null;
            foreach (var value in GetValues(item, out arg))
            {
                //Add marker to set clause
                if (values == null) values = new StringBuilder(value.Item1 + " = " + value.Item2);
                else values.Append(", " + value.Item1 + " = " + value.Item2);

                // If field is not a primary keys go to next field.
                if (!value.Item3) continue;

                // Add field to where clause.
                if (keys == null) keys = new StringBuilder(value.Item1 + " = " + value.Item2);
                else keys.Append(" AND " + value.Item1 + " = " + value.Item2);
            }
            if (keys == null) throw new NotSupportedException(string.Format("Can not update class without primary key. Class:{0}", item.GetType().Name));
            return string.Format(@"UPDATE {0} SET {1} WHERE {2};", tableName, values, keys);
        }

        public string Delete(TI item, out IList<Tuple<string, object, object[]>> arg)
        {
            var tableName = GetTableName();
            StringBuilder keys = null;
            //For each primary keys
            foreach (var value in GetValues(item, out arg))
            {
                // If not primary key
                if (!value.Item3)
                {
                    // Remove marker unused.
                    arg.Remove(arg.First(_ => string.Concat("@", _.Item1) == value.Item2));
                }
                else
                {
                    // Add field to where clause.
                    if (keys == null) keys = new StringBuilder(value.Item1 + " = " + value.Item2);
                    else keys.Append(" AND " + value.Item1 + " = " + value.Item2);
                }
            }
            if (keys == null) throw new NotSupportedException(string.Format("Can not delete class without primary key. Class:{0}", item.GetType().Name));
            return string.Format(@"DELETE FROM {0} WHERE {1};", tableName, keys);
        }

        private string GetTableName()
        {
            var tableAttribute = typeof(T).GetCustomAttributes(typeof(DataContractAttribute), true)
                                                            .Cast<DataContractAttribute>()
                                                            .FirstOrDefault();
            if (tableAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no DataContract attribute.", typeof(T)));
            return tableAttribute.Name ?? typeof(T).Name;
        }

        private List<Tuple<string, string, bool>> GetValues(object o, out IList<Tuple<string, object, object[]>> arg)
        {
            var result = new List<Tuple<string, string, bool>>();
            arg = new List<Tuple<string, object, object[]>>();
            
            //Get all private and public properties
            var propList = GetPropertiesList(true);

            //Foreach properties
            foreach (var prop in propList)
            {
                var customAttr = prop.GetCustomAttributes(true);
                var fieldAttribute = customAttr.OfType<DataMemberAttribute>().FirstOrDefault();
                if (fieldAttribute == null) continue;

                //Add sql name field and associated value.
                result.Add(new Tuple<string, string, bool>(fieldAttribute.Name ?? prop.Name, // Sql column name
                                                           string.Concat("@", prop.Name), // Marker to set parameter after
                                                           customAttr.OfType<SqlPrimaryKeyAttribute>().FirstOrDefault() != null)); // True if primary key, false else
                arg.Add(new Tuple<string, object, object[]>(prop.Name, // Marker to set parameter after
                                                            o != null ? prop.GetValue(o, null) : null, // Value to set parameter after
                                                            customAttr)); // Custom attribute for futher convertion
            }

            return result;
        }

        internal static IEnumerable<PropertyInfo> GetPropertiesList(bool keepOnlyDataMember)
        {
            return GetPropertiesList(typeof(T), keepOnlyDataMember);
        }

        private static IEnumerable<PropertyInfo> GetPropertiesList(Type t, bool keepOnlyDataMember)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => !keepOnlyDataMember || item.IsDefined(typeof(DataMemberAttribute), true))
                    .Union(t.BaseType != null ? GetPropertiesList(t.BaseType, keepOnlyDataMember) : new PropertyInfo[0]);
        }

        public string ToSqlType(Type objectType)
        {
            if (objectType == typeof(ushort)
                || objectType == typeof(short)
                || objectType == typeof(uint)
                || objectType == typeof(int)
                || objectType == typeof(ulong)
                || objectType == typeof(long)) return "integer";
            if (objectType == typeof(double)
                || objectType == typeof(float)) return "real";
            return "text";
        }
    }
}
