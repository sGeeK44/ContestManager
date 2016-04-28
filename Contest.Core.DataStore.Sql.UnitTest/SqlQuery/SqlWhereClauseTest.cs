using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class SqlWhereClauseTest : SqlQueryTestBase
    {
        private const string SQL_VALUE = "####";

        [SetUp]
        public override void Init()
        {
            SqlStrategy = new Mock<ISqlProviderStrategy>();
            SqlStrategy.Setup(_ => _.ToSqlValue(It.IsAny<object>(), It.IsAny<object[]>())).Returns(SQL_VALUE);
        }

        [TestCase]
        public void GetColumnName_NoOverrideNameEntity_ShouldReturnPropName()
        {
            var query = CreateSqlWhereClause<BasicEntity>(_ => true);
            var colunmName = query.GetColumnName(_ => _.Name);
            Assert.AreEqual("Name", colunmName);
        }

        [TestCase]
        public void GetColumnName_OverrideNameEntity_ShouldReturnAttributeNameProperty()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => true);
            var colunmName = query.GetColumnName(_ => _.Name);
            Assert.AreEqual("NAME", colunmName);
        }

        [TestCase]
        public void ToStatement_TruePredicate_ShouldReturnNull()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => true);

            Assert.AreEqual(string.Empty, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ById()
        {
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id);
            var expected = string.Format("WHERE ID = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ByVarId()
        {
            var id = OverrideNameEntity.Guid;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == id);
            var expected = string.Format("WHERE ID = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ByName()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Name == "NameSearch");
            var expected = string.Format("WHERE NAME = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_And()
        {
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id && _.Name == "NameSearch");
            var expected = string.Format("WHERE ID = {0} AND NAME = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_Or()
        {
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id || _.Name == "NameSearch");
            var expected = string.Format("WHERE ID = {0} OR NAME = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_AddOperator()
        {
            var param = 5;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age == param + 5);
            var expected = string.Format("WHERE AGE = {0} + {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_SubtractOperator()
        {
            var param = 5;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age == param - 5);
            var expected = string.Format("WHERE AGE = {0} - {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_NegateMember()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => -(_.Age) == 5);
            var expected = string.Format("WHERE - AGE = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_MultiplyMember()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age * 5 == 5);
            var expected = string.Format("WHERE AGE * {0} = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_DivideMember()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age / 5 == 5);
            var expected = string.Format("WHERE AGE / {0} = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ModuloMember()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age % 5 == 5);
            var expected = string.Format("WHERE AGE MOD {0} = {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_LessThanExpression()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age < 5);
            var expected = string.Format("WHERE AGE < {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_LessThanOrEqualExpression()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age <= 5);
            var expected = string.Format("WHERE AGE <= {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_MoreThanExpression()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age > 5);
            var expected = string.Format("WHERE AGE > {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_MoreThanOrEqualExpression()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age >= 5);
            var expected = string.Format("WHERE AGE >= {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_DifferentExpression()
        {
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age != 5);
            var expected = string.Format("WHERE AGE <> {0}", SQL_VALUE);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_NegateExpression()
        {
            IList<SqlFieldInfo> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => !_.Active);

            Assert.AreEqual("WHERE NOT ACTIVE", query.ToStatement());
        }

        public SqlWhereClause<T> CreateSqlWhereClause<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return new SqlWhereClause<T>(SqlStrategy.Object, predicate);
        }
    }
}
