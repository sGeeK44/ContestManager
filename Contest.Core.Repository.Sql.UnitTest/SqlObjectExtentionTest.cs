using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlObjectExtentionTest
    {
        [TestCase]
        public void GetNameByPropName()
        {
            var typeColunmName = SqlFieldExtension.ColumnName<Entity4, string>(_ => _.Prop1);
            Assert.AreEqual("Prop1", typeColunmName);
        }

        [TestCase]
        public void GetNameByPropNameOverride()
        {
            var typeColunmName = SqlFieldExtension.ColumnName<Entity4, string>(_ => _.Prop2);
            Assert.AreEqual("PROP2", typeColunmName);
        }

        [TestCase]
        public void GetNameByPropNameOnInterface()
        {
            var typeColunmName = SqlFieldExtension.ColumnName<IEntity4, string>(_ => _.Prop1);
            Assert.AreEqual("Prop1", typeColunmName);
        }

        [TestCase]
        public void GetNameByPropNameOverrideOnInterface()
        {
            var typeColunmName = SqlFieldExtension.ColumnName<IEntity4, string>(_ => _.Prop2);
            Assert.AreEqual("PROP2", typeColunmName);
        }
    }
}
