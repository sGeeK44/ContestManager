using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlObjectExtentionTest
    {
        [TestCase]
        public void GetNameByFieldName()
        {
            var ent = new Entity4();
            var typeColunmName = ent.ColumnName(_ => _.Field1);
            Assert.AreEqual("Field1", typeColunmName);
        }
        [TestCase]
        public void GetNameByFieldNameOverride()
        {
            var ent = new Entity4();
            var typeColunmName = ent.ColumnName(_ => _.Field2);
            Assert.AreEqual("FIELD2", typeColunmName);
        }
        [TestCase]
        public void GetNameByPropName()
        {
            var typeColunmName = SqlObjectExtension.ColumnName<Entity4, string>(_ => _.Prop1);
            Assert.AreEqual("Prop1", typeColunmName);
        }
        [TestCase]
        public void GetNameByPropNameOverride()
        {
            var typeColunmName = SqlObjectExtension.ColumnName<Entity4, string>(_ => _.Prop2);
            Assert.AreEqual("PROP2", typeColunmName);
        }
        [TestCase]
        public void GetNameByPropNameOnInterface()
        {
            var typeColunmName = SqlObjectExtension.ColumnName<IEntity4, string>(_ => _.Prop1);
            Assert.AreEqual("Prop1", typeColunmName);
        }
        [TestCase]
        public void GetNameByPropNameOverrideOnInterface()
        {
            var typeColunmName = SqlObjectExtension.ColumnName<IEntity4, string>(_ => _.Prop2);
            Assert.AreEqual("PROP2", typeColunmName);
        }
    }
}
