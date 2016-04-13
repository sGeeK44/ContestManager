using System.Data;

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

        public override IDataReader Execute(string request)
        {
            throw new System.NotImplementedException();
        }

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

        public override void OpenDatabase()
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(System.Collections.Generic.IList<string> requestList)
        {
            throw new System.NotImplementedException();
        }

        public override void CloseDatabase()
        {
            throw new System.NotImplementedException();
        }
    }
}
