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
using Contest.Core.DataStore;
using Contest.Core.DataStore.Sqlite;
using Contest.Core.Serialization;

namespace Contest.Core.Repository.Sql
{
    public class SqlBuilder<T,TI> : ISqlBuilder<T, TI> where T : class, TI
        where TI : class
    {
        private ISqlDataStore DataStore { get; set; }
        private IConverter Instance { get; set; }
        private IEnumConverter PivotEnumConverter { get; set; }

        public SqlBuilder()
        {
            DataStore = new SqliteDataStore();
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
            foreach (var propertyInfo in SqlObjectExtension.GetPropertiesList<T>())
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
            foreach (var prop in SqlObjectExtension.GetPropertiesList<T>())
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
            var fieldList = (from propertyInfo in SqlObjectExtension.GetPropertiesList<T>()
                             let fieldAttribute = propertyInfo.GetCustomAttributes(typeof(DataMemberAttribute), true)
                                                             .Cast<DataMemberAttribute>()
                                                             .FirstOrDefault()
                             where fieldAttribute != null
                             select string.Format("{0} {1}{2}",
                                                 fieldAttribute.Name ?? propertyInfo.Name,
                                                 DataStore.ToSqlType(propertyInfo.PropertyType),
                                                 propertyInfo.GetCustomAttributes(typeof(SqlPrimaryKeyAttribute), true)
                                                             .Cast<SqlPrimaryKeyAttribute>()
                                                             .FirstOrDefault() != null ? " primary key" : string.Empty)).ToList();


            if (fieldList.Count == 0) throw new NotSupportedException("Table have to own one field at less.");

            return string.Format(@"CREATE TABLE IF NOT EXISTS {0} ({1});", tableName, fieldList.Aggregate((x, y) => string.Concat(x, ", ", y)));
        }

        public string Select(Expression<Func<TI, bool>> predicate, out IList<SqlField> arg)
        {
            var tableName = GetTableName();
            StringBuilder columns = null;

            arg = SqlObjectExtension.GetSqlField<T>(null);
            foreach (var value in arg)
            {
                //Add marker to set clause
                if (columns == null) columns = new StringBuilder(value.ColumnName);
                else columns.Append(", " + value.ColumnName);
            }

            var whereSqlExpression = new WhereSqlExpression<T, TI>(predicate);
            var where = whereSqlExpression.ToStatement(out arg);
            return string.Format(@"SELECT {0} FROM {1}{2};", columns, tableName, string.IsNullOrEmpty(where) ? string.Empty : " " + where);
        }

        public string Insert(TI item, out IList<SqlField> arg)
        {
            var tableName = GetTableName();
            StringBuilder columnsName = null;
            StringBuilder columnsValue = null;
            arg = SqlObjectExtension.GetSqlField<T>(item);
            foreach (var value in arg)
            {
                //Add field name
                if (columnsName == null) columnsName = new StringBuilder(value.ColumnName);
                else columnsName.Append(", " + value.ColumnName);

                // Add marker to values.
                if (columnsValue == null) columnsValue = new StringBuilder(value.MarkerValue);
                else columnsValue.Append(", " + value.MarkerValue);
            }
            return string.Format(@"INSERT INTO {0} ({1}) VALUES ({2});", tableName, columnsName, columnsValue);
        }

        public string Update(TI item, out IList<SqlField> arg)
        {
            var tableName = GetTableName();
            StringBuilder values = null;
            StringBuilder keys = null;
            arg = SqlObjectExtension.GetSqlField<T>(item);
            foreach (var value in arg)
            {
                if (value.IsPrimaryKey)
                {
                    // Add field to where clause.
                    if (keys == null) keys = new StringBuilder(value.ColumnName + " = " + value.MarkerValue);
                    else keys.Append(" AND " + value.ColumnName + " = " + value.MarkerValue);
                }
                else
                {
                    //Add marker to set clause
                    if (values == null) values = new StringBuilder(value.ColumnName + " = " + value.MarkerValue);
                    else values.Append(", " + value.ColumnName + " = " + value.MarkerValue);
                }
            }
            if (keys == null) throw new NotSupportedException(string.Format("Can not update class without primary key. Class:{0}", item.GetType().Name));
            return string.Format(@"UPDATE {0} SET {1} WHERE {2};", tableName, values, keys);
        }

        public string Delete(TI item, out IList<SqlField> arg)
        {
            var tableName = GetTableName();
            arg = SqlObjectExtension.GetSqlField<T>(item).Where(_ => _.IsPrimaryKey).ToList();
            StringBuilder keys = null;
            //For each primary keys
            foreach (var value in arg)
            {
                // Add field to where clause.
                if (keys == null) keys = new StringBuilder(value.ColumnName + " = " + value.MarkerValue);
                else keys.Append(" AND " + value.ColumnName + " = " + value.MarkerValue);
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
    }
}
