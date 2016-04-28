using System;
using System.Collections.Generic;
using System.Reflection;
using Contest.Core.DataStore.Sql.EntityInfo;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlReferenceInfoTest
    {
        public Mock<ISqlPropertyInfo> FirstKey { get; set; }
        public Mock<ISqlPropertyInfo> FirstReferenceKey { get; set; }
        public Mock<IList<ISqlPropertyInfo>> KeyList { get; set; }
        public Mock<IList<ISqlPropertyInfo>> ReferenceKeyList { get; set; }

        [SetUp]
        public void Init()
        {
            FirstKey = new Mock<ISqlPropertyInfo>();
            KeyList = new Mock<IList<ISqlPropertyInfo>>();
            KeyList.Setup(_ => _[0]).Returns(FirstKey.Object);
            FirstReferenceKey = new Mock<ISqlPropertyInfo>(); 
            ReferenceKeyList = new Mock<IList<ISqlPropertyInfo>>();
            ReferenceKeyList.Setup(_ => _[0]).Returns(FirstReferenceKey.Object);
        }

        [TestCase]
        public void GetPredicate_OneToManyEntity_ShouldReturnCorrectPredicate()
        {
            var obj = OneToManyEntity.Create();
            FirstKey.Setup(_ => _.PropertyInfo).Returns(typeof(OneToManyEntity).GetProperty("Id"));
            FirstReferenceKey.Setup(_ => _.PropertyInfo).Returns(typeof(ManyToOneEntity).GetProperty("OneToManyEntityId"));
            var fields = CreateSqlReferenceInfoTester(typeof(OneToManyEntity).GetProperty("EntityList"), typeof(ManyToOneEntity));

            var result = fields.GetLambdaExpression(obj);

            Assert.AreEqual("_ => (_.OneToManyEntityId == value(Contest.Core.DataStore.Sql.UnitTest.Entities.OneToManyEntity).Id)", result.ToString());
        }

        [TestCase]
        public void GetPredicate_ManyToOneEntity_ShouldReturnCorrectPredicate()
        {
            var obj = ManyToOneEntity.Create();
            FirstKey.Setup(_ => _.PropertyInfo).Returns(typeof(ManyToOneEntity).GetProperty("OneToManyEntityId"));
            FirstReferenceKey.Setup(_ => _.PropertyInfo).Returns(typeof(OneToManyEntity).GetProperty("Id"));
            var fields = CreateSqlReferenceInfoTester(typeof(ManyToOneEntity).GetProperty("Entity"), typeof(OneToManyEntity));

            var result = fields.GetLambdaExpression(obj);

            Assert.AreEqual("_ => (_.Id == value(Contest.Core.DataStore.Sql.UnitTest.Entities.ManyToOneEntity).OneToManyEntityId)", result.ToString());
        }

        private SqlReferenceInfoTester CreateSqlReferenceInfoTester(PropertyInfo prop, Type referenceType)
        {
            return new SqlReferenceInfoTester(prop, referenceType, ReferenceKeyList.Object, KeyList.Object);
        }
    }
}
