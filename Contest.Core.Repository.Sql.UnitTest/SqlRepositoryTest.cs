using System.Linq;
using Contest.Core.DataStore.Sql;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.DataStore.Sql.SqlQuery;
using Moq;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public class SqlRepositoryTestBase<T> where T : class, IQueryable
    {
        public Mock<IDataContext<T>> Context { get; set; }
        public Mock<ISqlQueryFactory<T>> SqlBuilder { get; set; }
        public Mock<ISqlUnitOfWorks> UnitOfWork { get; set; }
        public Mock<ISqlDataStore> SqlDataStore { get; set; }
        public SqlRepository<T, T> SqlRepository { get; set; }

        [SetUp]
        public void Init()
        {
            Context = new Mock<IDataContext<T>>();
            SqlBuilder = new Mock<ISqlQueryFactory<T>>();
            UnitOfWork = new Mock<ISqlUnitOfWorks>();
            SqlDataStore = new Mock<ISqlDataStore>();

            SqlRepository = new SqlRepository<T, T>(SqlDataStore.Object)
            {
                SqlQueryFactory = SqlBuilder.Object,
                Context = Context.Object
            };

            UnitOfWork.Setup(_ => _.SqlDataStore).Returns(SqlDataStore.Object);
        }
    }

    [TestFixture]
    public class SqlRepositoryTestPart1 : SqlRepositoryTestBase<Identifiable<object>>
    {
        [TestCase]
        public void Constructor_ShouldBeEmptyStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            SqlDataStore.Verify();
        }

        [TestCase]
        public void Insert_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Insert(obj));

            SqlRepository.Insert(obj);

            Context.Verify();
        }

        [TestCase]
        public void Insert_ValidObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Insert(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Insert_WithUnitOfWork_InnerStatementShouldNotBeUpdate()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Insert(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Insert_WithUnitOfWork_UnitOfWorkStatementShouldNotBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWork.Setup(_ => _.Insert(obj));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Insert(obj);

            UnitOfWork.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Insert(obj));

            SqlRepository.InsertOrUpdate(obj);

            Context.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.InsertOrUpdate(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_AlreadyPresentObject_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);
            Context.Setup(_ => _.Update(obj));

            SqlRepository.InsertOrUpdate(obj);

            Context.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_AlreadyPresentObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);

            SqlRepository.InsertOrUpdate(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_InnerStatementShouldNotBeUpdate()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.InsertOrUpdate(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_UnitOfWorkStatementShouldNotBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWork.Setup(_ => _.InsertOrUpdate(obj));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.InsertOrUpdate(obj);

            UnitOfWork.Verify();
        }

        [TestCase]
        public void Update_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Update(obj));

            SqlRepository.Update(obj);

            Context.Verify();
        }

        [TestCase]
        public void Update_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Update(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Update_WithUnitOfWork_InnerStatementShouldNotBeUpdate()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Update(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Update_WithUnitOfWork_UnitOfWorkStatementShouldNotBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWork.Setup(_ => _.Update(obj));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Update(obj);

            UnitOfWork.Verify();
        }

        [TestCase]
        public void Delete_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Delete(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Delete_AlreadyPresentObject_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);
            Context.Setup(_ => _.Delete(obj));

            SqlRepository.Delete(obj);

            Context.Verify();
        }

        [TestCase]
        public void Delete_AlreadyPresentObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);

            SqlRepository.Delete(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Delete_WithUnitOfWork_InnerStatementShouldNotBeUpdate()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Delete(obj);

            SqlDataStore.Verify();
        }

        [TestCase]
        public void Delete_WithUnitOfWork_UnitOfWorkStatementShouldNotBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWork.Setup(_ => _.Delete(obj));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Delete(obj);

            UnitOfWork.Verify();
        }

        [TestCase]
        public void FillOneToManyReferences_RepositoryNoLinkedToUnitOfWork_ShouldDoNothing()
        {
            SqlRepository.TryFillOneToManyReference(new Identifiable<object>());
        }

        [TestCase]
        public void FillOneToManyReferences_NoOnToManyReference_ShouldDoNothing()
        {
            SqlRepository.UnitOfWorks = UnitOfWork.Object;
            SqlRepository.TryFillOneToManyReference(new Identifiable<object>());
        }
    }

    [TestFixture]
    public class SqlRepositoryTestPart2 : SqlRepositoryTestBase<OneToManyEntity>
    {
        [TestCase]
        public void FillOneToManyReferences_OneToManyReference_ShouldFillIt()
        {
            SqlRepository.UnitOfWorks = UnitOfWork.Object;
            var expectedReference = new ManyToOneEntity();
            var obj = new OneToManyEntity();

            SqlRepository.TryFillOneToManyReference(obj);

            Assert.AreEqual(expectedReference, obj.EntityList.ToList()[0]);
        }
    }
}
