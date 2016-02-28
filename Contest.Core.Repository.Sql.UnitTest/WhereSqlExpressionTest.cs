﻿using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class WhereSqlExpressionTest
    {
        [TestCase]
        public void True()
        {
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => true);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual(string.Empty, query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(0, arg.Count);
        }

        [TestCase]
        public void ById()
        {
            var ent = Entity1.CreateMock();
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => _.Id == ent.Id);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ByVarId()
        {
            var id = Entity1.Guid;
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => _.Id == id);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ByName()
        {
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE NAME = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, "NameSearch");
        }

        [TestCase]
        public void And()
        {
            var ent = Entity1.CreateMock();
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => _.Id == ent.Id && _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@ AND NAME = @P1@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void Or()
        {
            var ent = Entity1.CreateMock();
            var builder = new WhereSqlExpression<Entity1, Entity1>(_ => _.Id == ent.Id || _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@ OR NAME = @P1@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void TrueWithInterface()
        {
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => true);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual(string.Empty, query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(0, arg.Count);
        }

        [TestCase]
        public void ByIdWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => _.Id == ent.Id);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ByVarIdWithInterface()
        {
            var id = Entity5.Guid;
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => _.Id == id);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void ByNameWithInterface()
        {
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE NAME = @P0@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, "NameSearch");
        }

        [TestCase]
        public void AndWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => _.Id == ent.Id && _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@ AND NAME = @P1@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void OrWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, IEntity5>(_ => _.Id == ent.Id || _.Name == "NameSearch");
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE ID = @P0@ OR NAME = @P1@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void ToStatement_AddOperator_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            int param = 5;
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age == param + 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE = @P0@ + @P1@", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, 5);
            AssertArg(arg, 1, 5);
        }

        [TestCase]
        public void ToStatement_SubtractOperator_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            int param = 5;
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age == param - 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE = @P0@ - @P1@", query);
            Assert.IsNotNull(arg);
        }

        [TestCase]
        public void ToStatement_NegateMember_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => -(_.Age) == 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE - AGE = @P0@", query);
        }

        [TestCase]
        public void ToStatement_MultiplyMember_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age * 5 == 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE * @P0@ = @P1@", query);
        }

        [TestCase]
        public void ToStatement_DivideMember_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age / 5 == 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE / @P0@ = @P1@", query);
        }

        [TestCase]
        public void ToStatement_ModuloMember_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age % 5 == 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE MOD @P0@ = @P1@", query);
        }

        [TestCase]
        public void ToStatement_LessThanExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age < 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE < @P0@", query);
        }

        [TestCase]
        public void ToStatement_LessThanOrEqualExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age <= 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE <= @P0@", query);
        }

        [TestCase]
        public void ToStatement_MoreThanExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age > 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE > @P0@", query);
        }

        [TestCase]
        public void ToStatement_MoreThanOrEqualExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age >= 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE >= @P0@", query);
        }

        [TestCase]
        public void ToStatement_DifferentExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => _.Age != 5);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE AGE <> @P0@", query);
        }

        [TestCase]
        public void ToStatement_NegateExpression_ShouldReturnRightWereExpression()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new WhereSqlExpression<Entity5, Entity5>(_ => !_.Active);
            IList<Tuple<string, object, object[]>> arg;
            var query = builder.ToStatement(out arg);
            Assert.AreEqual("WHERE NOT ACTIVE", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(0, arg.Count);
        }

        private void AssertArg(IList<Tuple<string, object, object[]>> arg, int index, object expectedValue)
        {
            Assert.IsNotNull(arg[index]);
            Assert.AreEqual("P" + index.ToString(CultureInfo.InvariantCulture), arg[index].Item1);
            Assert.AreEqual(expectedValue, arg[index].Item2);
        }
    }
}
