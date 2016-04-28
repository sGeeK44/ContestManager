using System;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
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
            Assert.Throws<NotSupportedException>(() => EntityInfoFactory.GetEntityInfo<object>());
        }

        [TestCase]
        public void GetSqlEntity_ClassMarked_ShouldReturnNotNullElement()
        {
            Assert.IsNotNull(EntityInfoFactory.GetEntityInfo<BasicEntity>());
        }

        [TestCase]
        public void TableName_NoOverrideNameEntity_ShouldReturnClassName()
        {
            var entityInfo = EntityInfoFactory.GetEntityInfo<NoOverrideNameEntity>();
            Assert.AreEqual("NoOverrideNameEntity", entityInfo.TableName);
        }

        [TestCase]
        public void TableName_OverrideNameEntity_ShouldReturnSqlEntityAttributeName()
        {
            var entityInfo = EntityInfoFactory.GetEntityInfo<OverrideNameEntity>();
            Assert.AreEqual("ENTITY_1", entityInfo.TableName);
        }

        [TestCase]
        public void TableName_RelationEntity_ShouldReturnConcatenedValue()
        {
            var entityInfo = EntityInfoFactory.GetEntityInfo<Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>>();
            Assert.AreEqual("R_ManyToManyFirstEntity_ManyToManySecondEntity", entityInfo.TableName);
        }
    }
}
