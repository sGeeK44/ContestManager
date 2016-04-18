using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlFieldInfoTest
    {
        [TestCase]
        public void GetSqlField_BasicEntity_AllMarkedProperty()
        {
            var fields = SqlFieldInfo.GetSqlField<BasicEntity>();
            Assert.AreEqual(3, fields.Count);
        }

        [TestCase]
        public void ColumnName_NoOverrideNameEntity_ShouldReturnPropName()
        {
            var colunmName = SqlFieldInfo.GetColumnName<NoOverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("Name", colunmName);
        }

        [TestCase]
        public void ColumnName_OverrideNameEntity_ShouldReturnAttributeNameProperty()
        {
            var colunmName = SqlFieldInfo.GetColumnName<OverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("NAME", colunmName);
        }
    }
}
