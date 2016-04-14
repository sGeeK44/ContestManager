using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class SqlWhereClauseTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_TruePredicate_ShouldReturnNull()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => true);

            Assert.AreEqual(null, query.ToStatement(out arg));
            AssertArg(arg);
        }

        [TestCase]
        public void ToStatement_ById()
        {
            IList<SqlColumnField> arg;
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id);

            Assert.AreEqual("WHERE ID = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ToStatement_ByVarId()
        {
            var id = OverrideNameEntity.Guid;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == id);
            IList<SqlColumnField> arg;

            Assert.AreEqual("WHERE ID = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ToStatement_ByName()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Name == "NameSearch");

            Assert.AreEqual("WHERE NAME = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, "NameSearch");
        }

        [TestCase]
        public void ToStatement_And()
        {
            IList<SqlColumnField> arg;
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id && _.Name == "NameSearch");

            Assert.AreEqual("WHERE ID = @P0@ AND NAME = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"), "NameSearch");
        }

        [TestCase]
        public void ToStatement_Or()
        {
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Id == ent.Id || _.Name == "NameSearch");
            IList<SqlColumnField> arg;

            Assert.AreEqual("WHERE ID = @P0@ OR NAME = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"),"NameSearch");
        }

        [TestCase]
        public void ToStatement_TrueWithInterface()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => true);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1;", query.ToStatement(out arg));
            AssertArg(arg);
        }

        [TestCase]
        public void ToStatement_ByIdWithInterface()
        {
            IList<SqlColumnField> arg;
            IOverrideNameEntity ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => _.Id == ent.Id);

            Assert.AreEqual("WHERE ID = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ToStatement_ByVarIdWithInterface()
        {
            IList<SqlColumnField> arg;
            var id = OverrideNameEntity.Guid;
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => _.Id == id);

            Assert.AreEqual("WHERE ID = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ToStatement_ByNameWithInterface()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => _.Name == "NameSearch");

            Assert.AreEqual("WHERE NAME = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, "NameSearch");
        }

        [TestCase]
        public void ToStatement_AndWithInterface()
        {
            IList<SqlColumnField> arg;
            IOverrideNameEntity ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => _.Id == ent.Id && _.Name == "NameSearch");

            Assert.AreEqual("WHERE ID = @P0@ AND NAME = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"),"NameSearch");
        }

        [TestCase]
        public void ToStatement_OrWithInterface()
        {
            IList<SqlColumnField> arg;
            IOverrideNameEntity ent = OverrideNameEntity.CreateMock();
            var query = CreateSqlWhereClause<OverrideNameEntity, IOverrideNameEntity>(_ => _.Id == ent.Id || _.Name == "NameSearch");

            Assert.AreEqual("WHERE ID = @P0@ OR NAME = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"), "NameSearch");
        }

        [TestCase]
        public void ToStatement_AddOperator()
        {
            var param = 5;
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age == param + 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE = @P0@ + @P1@;", query.ToStatement(out arg));
            AssertArg(arg, 5, 5);
        }

        [TestCase]
        public void ToStatement_SubtractOperator()
        {
            var param = 5;
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age == param - 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE = @P0@ - @P1@;", query.ToStatement(out arg));
            AssertArg(arg, 5, 5);
        }

        [TestCase]
        public void ToStatement_NegateMember()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => -(_.Age) == 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE - AGE = @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_MultiplyMember()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age * 5 == 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE * @P0@ = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, 5, 5);
        }

        [TestCase]
        public void ToStatement_DivideMember()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age / 5 == 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE / @P0@ = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, 5, 5);
        }

        [TestCase]
        public void ToStatement_ModuloMember()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age % 5 == 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE MOD @P0@ = @P1@;", query.ToStatement(out arg));
            AssertArg(arg, 5, 5);
        }

        [TestCase]
        public void ToStatement_LessThanExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age < 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE < @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_LessThanOrEqualExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age <= 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE <= @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_MoreThanExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age > 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE > @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_MoreThanOrEqualExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age >= 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE >= @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_DifferentExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => _.Age != 5);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE AGE <> @P0@;", query.ToStatement(out arg));
            AssertArg(arg, 5);
        }

        [TestCase]
        public void ToStatement_NegateExpression()
        {
            IList<SqlColumnField> arg;
            var query = CreateSqlWhereClause<OverrideNameEntity>(_ => !_.Active);

            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE NOT ACTIVE;", query.ToStatement(out arg));
            AssertArg(arg);
        }

        public void AssertArg(IList<SqlColumnField> arg, params object[] expectedValue)
        {
            Assert.IsNotNull(arg);
            Assert.AreEqual(expectedValue.Length, arg.Count);
            for (int i = 0; i < expectedValue.Length; i++)
            {
                Assert.IsNotNull(arg[i]);
                Assert.AreEqual("@P" + i.ToString(CultureInfo.InvariantCulture) + "@", arg[i].MarkerValue);
                Assert.AreEqual(expectedValue, arg[i]);
            }
        }

        public SqlWhereClause<T, T> CreateSqlWhereClause<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return CreateSqlWhereClause<T, T>(predicate);
        }

        public SqlWhereClause<T, TI> CreateSqlWhereClause<T, TI>(Expression<Func<TI, bool>> predicate)
            where T : class, TI
            where TI : class
        {
            return new SqlWhereClause<T, TI>(predicate);
        }
    }
}
