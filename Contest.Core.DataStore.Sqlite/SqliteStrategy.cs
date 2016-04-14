using Contest.Core.Converters;
using Contest.Core.DataStore.Sql;

namespace Contest.Core.DataStore.Sqlite
{
    public class SqliteStrategy : SqlProviderStrategyBase
    {
        private const string INTEGER_COLUMN_VALUE = "integer";
        private const string REAL_COLUMN_VALUE = "real";
        private const string TEXT_COLUMN_VALUE = "text";

        protected override IConverter Converter { get { return Converters.Converter.Instance; } }

        /// <summary>
        /// Determine default Sql column type when .net is not managed
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDefaultColumnType() { return TEXT_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store string .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetStringColumnType() { return TEXT_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store decimal .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDecimalColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store float .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetFloatColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store double .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetDoubleColumnType() { return REAL_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store long .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetLongColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store ulong .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedLongColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store uint .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedIntColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store int .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetIntColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store short .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetShortColumnType() { return INTEGER_COLUMN_VALUE; }

        /// <summary>
        /// Determine Sql column type to store ushort .Net value
        /// </summary>
        /// <returns>A string which contains Sql column type</returns>
        protected override string GetUnsignedShortColumnType() { return INTEGER_COLUMN_VALUE; }
    }
}
