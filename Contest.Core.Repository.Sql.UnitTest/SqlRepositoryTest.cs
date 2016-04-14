using Contest.Core.DataStore;
using Contest.Core.DataStore.Sql;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.DataStore.Sql.SqlQuery;
using Moq;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlRepositoryTest
    {
        private Mock<IDataContext<Identifiable<object>>> Context { get; set; }
        private Mock<ISqlQueryFactory<Identifiable<object>>> SqlBuilder { get; set; }
        private Mock<ISqlUnitOfWorks> UnitOfWork { get; set; }
        private Mock<ISqlDataStore> SqlDataStore { get; set; }
        private SqlRepository<Identifiable<object>, Identifiable<object>> SqlRepository { get; set; }

        [SetUp]
        public void Init()
        {
            Context = new Mock<IDataContext<Identifiable<object>>>();
            SqlBuilder = new Mock<ISqlQueryFactory<Identifiable<object>>>();
            UnitOfWork = new Mock<ISqlUnitOfWorks>();
            SqlDataStore = new Mock<ISqlDataStore>();

            SqlRepository = new SqlRepository<Identifiable<object>, Identifiable<object>>(SqlDataStore.Object)
            {
                SqlQueryFactory = SqlBuilder.Object,
                Context = Context.Object
            };
        }

        [TestCase]
        public void Constructor_ShouldBeEmptyStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var repo = new SqlRepository<Identifiable<object>, Identifiable<object>>(SqlDataStore.Object);

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
    }
}
