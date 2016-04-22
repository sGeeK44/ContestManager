using System;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlEntityInfoTest
    {
        [TestCase]
        public void GetSqlEntity_ClassNotMarked_ShouldThrowException()
        {
            Assert.Throws<NotSupportedException>(() => SqlEntityInfo.GetEntityInfo<object>());
        }

        [TestCase]
        public void GetSqlEntity_ClassMarked_ShouldReturnNotNullElement()
        {
            Assert.IsNotNull(SqlEntityInfo.GetEntityInfo<BasicEntity>());
        }

        [TestCase]
        public void TableName_NoOverrideNameEntity_ShouldReturnClassName()
        {
            var entityInfo = SqlEntityInfo.GetEntityInfo<NoOverrideNameEntity>();
            Assert.AreEqual("NoOverrideNameEntity", entityInfo.TableName);
        }

        [TestCase]
        public void TableName_OverrideNameEntity_ShouldReturnSqlEntityAttributeName()
        {
            var entityInfo = SqlEntityInfo.GetEntityInfo<OverrideNameEntity>();
            Assert.AreEqual("ENTITY_1", entityInfo.TableName);
        }
    }
}
