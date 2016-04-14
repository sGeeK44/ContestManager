using Contest.Core.DataStore.Sql;

namespace Contest.Core.DataStore.UnitTest
{
    public class SqlProviderStrategyBaseTester : SqlProviderStrategyBase
    {
        public bool GetDefaultColumnTypeHasCalled { get; set; }
        public bool GetDecimalColumnTypeHasCalled { get; set; }
        public bool GetFloatColumnTypeHasCalled { get; set; }
        public bool GetDoubleColumnTypeHasCalled { get; set; }
        public bool GetLongColumnTypeHasCalled { get; set; }
        public bool GetUnsignedLongColumnTypeHasCalled { get; set; }
        public bool GetUnsignedIntColumnTypeHasCalled { get; set; }
        public bool GetIntColumnTypeHasCalled { get; set; }
        public bool GetShortColumnTypeHasCalled { get; set; }
        public bool GetUnsignedShortColumnTypeHasCalled { get; set; }
        public bool GetStringColumnTypeHasCalled { get; set; }

        protected override string GetDefaultColumnType() { GetDefaultColumnTypeHasCalled = true; return null; }
        protected override string GetDecimalColumnType() { GetDecimalColumnTypeHasCalled = true; return null; }
        protected override string GetFloatColumnType() { GetFloatColumnTypeHasCalled = true; return null; }
        protected override string GetDoubleColumnType() { GetDoubleColumnTypeHasCalled = true; return null; }
        protected override string GetLongColumnType() { GetLongColumnTypeHasCalled = true; return null; }
        protected override string GetUnsignedLongColumnType() { GetUnsignedLongColumnTypeHasCalled = true; return null; }
        protected override string GetUnsignedIntColumnType() { GetUnsignedIntColumnTypeHasCalled = true; return null; }
        protected override string GetIntColumnType() { GetIntColumnTypeHasCalled = true; return null; }
        protected override string GetShortColumnType() { GetShortColumnTypeHasCalled = true; return null; }
        protected override string GetUnsignedShortColumnType() { GetUnsignedShortColumnTypeHasCalled = true; return null; }
        protected override string GetStringColumnType() { GetStringColumnTypeHasCalled = true; return null; }

        protected override Converters.IConverter Converter
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
