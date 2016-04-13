using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlBuilderTest
    {
        [TestCase]
        public void CreateTable()
        {
            var builder = new SqlBuilder<Entity1>();
            var query = builder.CreateTable();
            Assert.AreEqual("CREATE TABLE IF NOT EXISTS ENTITY_1 (ID text primary key, NAME text, ACTIVE text, AGE integer);", query);
        }

        [TestCase]
        public void CreateTable_WithNoNameOnDataMemberAttr()
        {
            var builder = new SqlBuilder<Entity6>();
            var query = builder.CreateTable();
            Assert.AreEqual("CREATE TABLE IF NOT EXISTS Entity6 (Name text);", query);
        }

        [TestCase]
        public void SqliteInsertRow()
        {
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Insert(Entity1.CreateMock(), out arg);
            Assert.AreEqual("INSERT INTO ENTITY_1 (ID, NAME, ACTIVE, AGE) VALUES (@ID@, @NAME@, @ACTIVE@, @AGE@);", query);
        }

        [TestCase]
        public void SqliteInsertRowWithInterface()
        {
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Insert(Entity5.CreateMock(), out arg);
            Assert.AreEqual("INSERT INTO ENTITY_5 (ID, NAME, ACTIVE, AGE) VALUES (@ID@, @NAME@, @ACTIVE@, @AGE@);", query);
        }

        [TestCase]
        public void SqliteUpdateRowWithoutPrimaryKey()
        {
            var builder = new SqlBuilder<Entity3, Entity3>();
            var ent = Entity3.CreateMock();
            IList<SqlField> arg;
            Assert.Throws<NotSupportedException>(() => builder.Update(ent, out arg));
        }

        [TestCase]
        public void SqliteDeleteRowWithoutPrimaryKey()
        {
            var builder = new SqlBuilder<Entity3, Entity3>();
            var ent = Entity3.CreateMock();
            IList<SqlField> arg;
            Assert.Throws< NotSupportedException>(() => builder.Delete(ent, out arg));
        }

        [TestCase]
        public void SqliteUpdateRow()
        {
            var ent = Entity1.CreateMock();
            ent.Name += "New";
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Update(ent, out arg);
            Assert.AreEqual("UPDATE ENTITY_1 SET NAME = @NAME@, ACTIVE = @ACTIVE@, AGE = @AGE@ WHERE ID = @ID@;", query);
        }

        [TestCase]
        public void SqliteSelectTrue()
        {
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => true, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(0, arg.Count);
        }

        [TestCase]
        public void SqliteSelectRowById()
        {
            var ent = Entity1.CreateMock();
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void SqliteSelectRowByVarId()
        {
            var id = Entity1.Guid;
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == id, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void SqliteSelectRowByName()
        {
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE NAME = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, "NameSearch");
        }

        [TestCase]
        public void SqliteSelectAnd()
        {
            var ent = Entity1.CreateMock();
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id && _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = @P0@ AND NAME = @P1@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void SqliteSelectOr()
        {
            var ent = Entity1.CreateMock();
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id || _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = @P0@ OR NAME = @P1@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void SqliteDeleteRow()
        {
            var builder = new SqlBuilder<Entity1>();
            IList<SqlField> arg;
            var query = builder.Delete(Entity1.CreateMock(), out arg);
            Assert.AreEqual("DELETE FROM ENTITY_1 WHERE ID = @ID@;", query);
        }

        [TestCase]
        public void SqliteUpdateRowWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            ent.Name += "New";
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Update(ent, out arg);
            Assert.AreEqual("UPDATE ENTITY_5 SET NAME = @NAME@, ACTIVE = @ACTIVE@, AGE = @AGE@ WHERE ID = @ID@;", query);
        }

        [TestCase]
        public void SqliteSelectTrueWithInterface()
        {
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => true, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(0, arg.Count);
        }

        [TestCase]
        public void SqliteSelectRowByIdWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void SqliteSelectRowByVarIdWithInterface()
        {
            var id = Entity5.Guid;
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == id, out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        public void SqliteSelectRowByNameWithInterface()
        {
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE NAME = @P0@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(1, arg.Count);
            AssertArg(arg, 0, "NameSearch");
        }

        [TestCase]
        public void SqliteSelectAndWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id && _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = @P0@ AND NAME = @P1@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void SqliteSelectOrWithInterface()
        {
            IEntity5 ent = Entity5.CreateMock();
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Select(_ => _.Id == ent.Id || _.Name == "NameSearch", out arg);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = @P0@ OR NAME = @P1@;", query);
            Assert.IsNotNull(arg);
            Assert.AreEqual(2, arg.Count);
            AssertArg(arg, 0, new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
            AssertArg(arg, 1, "NameSearch");
        }

        [TestCase]
        public void SqliteDeleteRowWithInterface()
        {
            var builder = new SqlBuilder<Entity5, IEntity5>();
            IList<SqlField> arg;
            var query = builder.Delete(Entity5.CreateMock(), out arg);
            Assert.AreEqual("DELETE FROM ENTITY_5 WHERE ID = @ID@;", query);
        }

        private void AssertArg(IList<SqlField> arg, int index, object expectedValue)
        {
            Assert.IsNotNull(arg[index]);
            Assert.AreEqual("@P" + index.ToString(CultureInfo.InvariantCulture) + "@", arg[index].MarkerValue);
            Assert.AreEqual(expectedValue, arg[index].Value);
        }
    }
}
