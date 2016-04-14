using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlColumnFieldTest
    {
        [TestCase]
        public void ColumnName_NoOverrideNameEntity_ShouldReturnPropName()
        {
            var colunmName = SqlColumnField.GetColumnName<NoOverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("Name", colunmName);
        }

        [TestCase]
        public void ColumnName_OverrideNameEntity_ShouldReturnAttributeNameProperty()
        {
            var colunmName = SqlColumnField.GetColumnName<OverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("NAME", colunmName);
        }
    }
}
