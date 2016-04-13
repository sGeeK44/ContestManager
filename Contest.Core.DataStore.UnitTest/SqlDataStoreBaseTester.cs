using Contest.Core.Repository.Sql;

namespace Contest.Core.DataStore.UnitTest
{
    public class SqlDataStoreBaseTester : SqlDataStoreBase
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
        
        public override string GetDefaultColumnType() { GetDefaultColumnTypeHasCalled = true; return null; }
        public override string GetDecimalColumnType() { GetDecimalColumnTypeHasCalled = true; return null; }
        public override string GetFloatColumnType() { GetFloatColumnTypeHasCalled = true; return null; }
        public override string GetDoubleColumnType() { GetDoubleColumnTypeHasCalled = true; return null; }
        public override string GetLongColumnType() { GetLongColumnTypeHasCalled = true; return null; }
        public override string GetUnsignedLongColumnType() { GetUnsignedLongColumnTypeHasCalled = true; return null; }
        public override string GetUnsignedIntColumnType() { GetUnsignedIntColumnTypeHasCalled = true; return null; }
        public override string GetIntColumnType() { GetIntColumnTypeHasCalled = true; return null; }
        public override string GetShortColumnType() { GetShortColumnTypeHasCalled = true; return null; }
        public override string GetUnsignedShortColumnType() { GetUnsignedShortColumnTypeHasCalled = true; return null; }
        public override string GetStringColumnType() { GetStringColumnTypeHasCalled = true; return null; }
    }
}
