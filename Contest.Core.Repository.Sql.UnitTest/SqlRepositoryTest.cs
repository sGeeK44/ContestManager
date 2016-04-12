using System;
using System.Collections.Generic;
using Contest.Core.Converters;
using Moq;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlRepositoryTest
    {
        private Mock<IDataContext<Identifiable<object>>> Context { get; set; }
        private Mock<ISqlBuilder<Identifiable<object>, Identifiable<object>>> SqlBuilder { get; set; }
        private Mock<ISqlUnitOfWorks> UnitOfWork { get; set; }
        private Mock<IConverter> Converter { get; set; }
        private SqlRepository<Identifiable<object>, Identifiable<object>> SqlRepository { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            Context = new Mock<IDataContext<Identifiable<object>>>();
            SqlBuilder = new Mock<ISqlBuilder<Identifiable<object>, Identifiable<object>>>();
            UnitOfWork = new Mock<ISqlUnitOfWorks>();
            Converter = new Mock<IConverter>();
            SqlRepository = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = SqlBuilder.Object,
                Converter = Converter.Object,
                Context = Context.Object,
                UnitOfWorks = UnitOfWork.Object
            };
        }

        [TestCase]
        public void Create()
        {
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>();

            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
        }

        [TestCase]
        public void Insert_WithoutUnitOfWork()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new Identifiable<object>();

            repo.Insert(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Insert_WithUnitOfWork()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new Identifiable<object>();

            repo.Insert(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void InsertOrUpdate_WithoutUnitOfWork_1()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new Identifiable<object>();

            repo.InsertOrUpdate(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(0, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.AreEqual(0, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void InsertOrUpdate_WithoutUnitOfWork_2()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new Identifiable<object>();
            repo.Insert(obj);

            repo.InsertOrUpdate(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(1, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.AreEqual(1, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(2, repo.QueryList.Count);
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_1()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new Identifiable<object>();

            repo.Insert(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(0, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.AreEqual(0, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_2()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new Identifiable<object>();

            repo.Insert(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(0, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.AreEqual(0, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void Update_WithoutUnitOfWork()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new Identifiable<object>();

            repo.Update(obj);

            Assert.AreEqual(1, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Update_WithUnitOfWork()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new Identifiable<object>();

            repo.Update(obj);

            Assert.AreEqual(1, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void Delete_WithoutUnitOfWork()
        {
            var context = new ContextMock<Identifiable<object>>();
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new Identifiable<object>();

            repo.Delete(obj);

            Assert.AreEqual(1, context.CountDeleteCall);
            Assert.AreEqual(1, sqlBuilder.CountDeleteCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Delete_WithUnitOfWork_ShouldUpdateContext()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Delete(obj));
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();
            
            SqlRepository.Delete(obj);

            Context.Verify();
        }

        [TestCase]
        public void Delete_WithUnitOfWork_ShouldUpdateQueryList()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Delete(obj));
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = Context.Object,
                UnitOfWorks = unitOfworks
            };

            repo.Delete(obj);

            Assert.AreEqual(1, sqlBuilder.CountDeleteCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void Delete_WithUnitOfWork_ShouldUpdateUnitOfWork()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Delete(obj));
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = Context.Object,
                UnitOfWorks = unitOfworks
            };

            repo.Delete(obj);

            Assert.AreEqual(1, sqlBuilder.CountDeleteCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void FirstOrDefault_ExistInCache()
        {
            var obj = new Identifiable<object>();
            var context = new ContextMock<Identifiable<object>> { Item = new List<Identifiable<object>> { obj } };
            var sqlBuilder = new SqlBuilderMock<Identifiable<object>, Identifiable<object>>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };

            var result = repo.FirstOrDefault(_ => _ == obj);

            Assert.AreEqual(obj, result);
        }

        [TestCase]
        public void PrepareSqlRequest_TrueCondition_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => true);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ById_ShouldRetunrValidSqlQuery()
        {
            var ent = Entity1.CreateMock();
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ByVarId_ShouldRetunrValidSqlQuery()
        {
            var id = Entity1.Guid;
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => _.Id == id);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ByName_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_And_ShouldRetunrValidSqlQuery()
        {
            var ent = Entity1.CreateMock();
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id && _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C' AND NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_Or_ShouldRetunrValidSqlQuery()
        {
            var ent = Entity1.CreateMock();
            var repo = CreateRepository<Entity1>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id || _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C' OR NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_TrueWithInterface_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => true);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ByIdWithInterface_ShouldRetunrValidSqlQuery()
        {
            IEntity5 ent = Entity5.CreateMock();
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ByVarIdWithInterface_ShouldRetunrValidSqlQuery()
        {
            var id = Entity5.Guid;
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => _.Id == id);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ByNameWithInterface_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_AndWithInterface_ShouldRetunrValidSqlQuery()
        {
            IEntity5 ent = Entity5.CreateMock();
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id && _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C' AND NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_OrWithInterface_ShouldRetunrValidSqlQuery()
        {
            IEntity5 ent = Entity5.CreateMock();
            var repo = CreateRepository<Entity5, IEntity5>();
            var query = repo.PrepareSqlRequest(_ => _.Id == ent.Id || _.Name == "NameSearch");
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE ID = '6A4A4F81-0C29-43C4-863E-AD10398B3A8C' OR NAME = 'NameSearch';", query);
        }

        [TestCase]
        public void PrepareSqlRequest_AddOperator_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            int param = 5;
            var query = repo.PrepareSqlRequest(_ => _.Age == param + 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE = 5 + 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_SubtractOperator_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            int param = 5;
            var query = repo.PrepareSqlRequest(_ => _.Age == param - 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE = 5 - 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_NegateMember_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => -(_.Age) == 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE - AGE = 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_MultiplyMember_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age * 5 == 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE * 5 = 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_DivideMember_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age / 5 == 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE / 5 = 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_ModuloMember_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age % 5 == 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE MOD 5 = 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_LessThanExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age < 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE < 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_LessThanOrEqualExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age <= 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE <= 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_MoreThanExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age > 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE > 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_MoreThanOrEqualExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age >= 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE >= 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_DifferentExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => _.Age != 5);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE AGE <> 5;", query);
        }

        [TestCase]
        public void PrepareSqlRequest_NegateExpression_ShouldRetunrValidSqlQuery()
        {
            var repo = CreateRepository<Entity5>();
            var query = repo.PrepareSqlRequest(_ => !_.Active);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_5 WHERE NOT ACTIVE;", query);
        }

        private SqlRepository<T, T> CreateRepository<T>() where T : class, IQueryable
        {
            return new SqlRepository<T, T> { SqlBuilder = new SqlBuilder<T, T>() };
        }

        private SqlRepository<T, TI> CreateRepository<T, TI>() where T : class, TI where TI : class, IQueryable
        {
            return new SqlRepository<T, TI> { SqlBuilder = new SqlBuilder<T, TI>() };
        }
    }
}
