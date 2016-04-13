﻿using System;
using System.Collections.Generic;
using System.Data;

namespace Contest.Core.DataStore
{
    public abstract class SqlDataStoreBase : ISqlDataStore
    {
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

        public abstract void OpenDatabase();
        public abstract void CloseDatabase();
        public abstract void Execute(IList<string> requestList);
        public abstract IDataReader Execute(string request);

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
