using System;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlOneToManyReferenceTest
    {
        [TestCase]
        public void GetSqlField_OneToManyEntity_AllMarkedProperty()
        {
            var fields = SqlOneToManyReferenceInfo.GetSqlReference<OneToManyEntity>();
            Assert.AreEqual(1, fields.Count);
        }

        [TestCase]
        public void GetSqlField_ReferencePropertiesIsNotAIList_ShouldThrowException()
        {
            Assert.Throws<NotSupportedException>(() => SqlOneToManyReferenceInfo.GetSqlReference<OneToManyWithoutIListEntity>());
        }

        [TestCase]
        public void GetPredicate_OneToManyEntity_ShouldReturnCorrectPredicate()
        {
            var obj = OneToManyEntity.Create();

            var fields = SqlOneToManyReferenceInfo.GetSqlReference<OneToManyEntity>();
            var result = fields[0].GetPredicate(obj);
            
            var provider = new Mock<ISqlProviderStrategy>();
            provider.Setup(_ => _.ToSqlValue(It.IsAny<object>(), It.IsAny<object[]>())).Returns("##");
            var whereClause = new SqlWhereClause<ManyToOneEntity>(provider.Object, result);
            var xx = whereClause.ToStatement();

            Assert.AreEqual("WHERE OneToManyEntityId = ##", xx);
        }
    }
}
