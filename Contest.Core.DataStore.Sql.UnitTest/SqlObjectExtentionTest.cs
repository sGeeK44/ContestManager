using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlObjectExtentionTest
    {
        [TestCase]
        public void ColumnName_NoOverrideNameEntity_ShouldReturnPropName()
        {
            var colunmName = SqlFieldExtension.ColumnName<NoOverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("Name", colunmName);
        }

        [TestCase]
        public void ColumnName_OverrideNameEntity_ShouldReturnAttributeNameProperty()
        {
            var colunmName = SqlFieldExtension.ColumnName<OverrideNameEntity, string>(_ => _.Name);
            Assert.AreEqual("NAME", colunmName);
        }
    }
}
