using System;
using Contest.Core.Converters;

namespace Contest.Core.DataStore.Sql
{
    public abstract class SqlProviderStrategyBase : ISqlProviderStrategy
    {
        protected abstract IConverter Converter { get; }

        /// <summary>
        /// Convert specified .Net type into Sql column type
        /// </summary>
        /// <param name="objectType">.Net object type</param>
        /// <returns>Sql column type equivalent</returns>
        public string ToSqlType(Type objectType)
        {
            if (objectType == typeof(ushort)) return GetUnsignedShortColumnType();
            if (objectType == typeof(short)) return GetShortColumnType();
            if (objectType == typeof(uint)) return GetUnsignedIntColumnType();
            if (objectType == typeof(int)) return GetIntColumnType();
            if (objectType == typeof(ulong)) return GetUnsignedLongColumnType();
            if (objectType == typeof(long)) return GetLongColumnType();
            if (objectType == typeof(double)) return GetDoubleColumnType();
            if (objectType == typeof(float)) return GetFloatColumnType();
            if (objectType == typeof(decimal)) return GetDecimalColumnType();
            if (objectType == typeof(string)) return GetStringColumnType();
            return GetDefaultColumnType();
        }
        
        public string ToSqlValue(object obj, object[] attr)
        {
            if (obj == null) return "NULL";
            var value = Converter.Convert(obj, attr);
            var objectType = obj.GetType();
            if (objectType == typeof(ushort)
                || objectType == typeof(short)
                || objectType == typeof(uint)
                || objectType == typeof(int)
                || objectType == typeof(ulong)
                || objectType == typeof(long)
                || objectType == typeof(float)) return value;

            return string.Concat("'", value.Replace("'", "''"), "'");
        }

        public object FromSqlValue(Type propertyType, string sqlValue, object[] attr)
        {
            return Converter.Convert(propertyType, sqlValue, attr);
        }

        /// <summary>
        /// Determine default Sql column type when .net is not managed
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetDefaultColumnType();

        /// <summary>
        /// Determine Sql column type to store string .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetStringColumnType();

        /// <summary>
        /// Determine Sql column type to store decimal .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetDecimalColumnType();

        /// <summary>
        /// Determine Sql column type to store float .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetFloatColumnType();

        /// <summary>
        /// Determine Sql column type to store double .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetDoubleColumnType();

        /// <summary>
        /// Determine Sql column type to store long .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetLongColumnType();

        /// <summary>
        /// Determine Sql column type to store ulong .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetUnsignedLongColumnType();

        /// <summary>
        /// Determine Sql column type to store uint .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetUnsignedIntColumnType();

        /// <summary>
        /// Determine Sql column type to store int .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetIntColumnType();

        /// <summary>
        /// Determine Sql column type to store short .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetShortColumnType();

        /// <summary>
        /// Determine Sql column type to store ushort .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected abstract string GetUnsignedShortColumnType();
    }
}
