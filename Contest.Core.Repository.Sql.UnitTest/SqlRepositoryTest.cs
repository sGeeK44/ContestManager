using System.Collections.Generic;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlRepositoryTest
    {
        [TestCase]
        public void Create()
        {
            var repo = new SqlRepository<object, object>();

            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
        }

        [TestCase]
        public void Insert_WithoutUnitOfWork()
        {
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new object();

            repo.Insert(obj);

            Assert.AreEqual(1, context.CountInsertCall);
            Assert.AreEqual(1, sqlBuilder.CountInsertCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Insert_WithUnitOfWork()
        {
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new object();

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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new object();

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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new object();
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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new object();

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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new object();

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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new object();

            repo.Update(obj);

            Assert.AreEqual(1, context.CountUpdateCall);
            Assert.AreEqual(1, sqlBuilder.CountUpdateCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Update_WithUnitOfWork()
        {
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new object();

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
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context
            };
            var obj = new object();

            repo.Delete(obj);

            Assert.AreEqual(1, context.CountDeleteCall);
            Assert.AreEqual(1, sqlBuilder.CountDeleteCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(1, repo.QueryList.Count);
        }

        [TestCase]
        public void Delete_WithUnitOfWork()
        {
            var context = new ContextMock<object>();
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };
            var obj = new object();

            repo.Delete(obj);

            Assert.AreEqual(1, context.CountDeleteCall);
            Assert.AreEqual(1, sqlBuilder.CountDeleteCall);
            Assert.IsNotNull(repo.QueryList);
            Assert.AreEqual(0, repo.QueryList.Count);
            Assert.AreEqual(1, unitOfworks.CountAddRequestCall);
        }

        [TestCase]
        public void FirstOrDefault_ExistInCache()
        {
            var obj = new object();
            var context = new ContextMock<object> { Item = new List<object> { obj } };
            var sqlBuilder = new SqlBuilderMock<object, object>();
            var unitOfworks = new SqlUnitOfWorksMock();

            var repo = new SqlRepository<object, object>
            {
                SqlBuilder = sqlBuilder,
                Converter = new ConverterMock(),
                Context = context,
                UnitOfWorks = unitOfworks
            };

            var result = repo.FirstOrDefault(_ => _ == obj);

            Assert.AreEqual(obj, result);
        }
    }
}
