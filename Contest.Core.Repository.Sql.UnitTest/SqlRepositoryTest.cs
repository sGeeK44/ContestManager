using System.Data;
using Contest.Core.DataStore.Sql;
using Contest.Core.DataStore.Sql.BusinessObjectFactory;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.DataStore.Sql.SqlQuery;
using Moq;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class SqlRepositoryTest
    {
        public Mock<IDataContext<Identifiable<object>>> Context { get; set; }
        public Mock<ISqlQueryFactory<Identifiable<object>>> SqlBuilder { get; set; }
        public Mock<ISqlUnitOfWorks> UnitOfWork { get; set; }
        public Mock<ISqlDataStore> SqlDataStore { get; set; }
        public Mock<IBusinessObjectFactory<Identifiable<object>>> BoFactory { get; set; }
        public SqlRepository<Identifiable<object>> SqlRepository { get; set; }

        [SetUp]
        public void Init()
        {
            Context = new Mock<IDataContext<Identifiable<object>>>();
            SqlBuilder = new Mock<ISqlQueryFactory<Identifiable<object>>>();
            UnitOfWork = new Mock<ISqlUnitOfWorks>();
            SqlDataStore = new Mock<ISqlDataStore>();
            BoFactory = new Mock<IBusinessObjectFactory<Identifiable<object>>>();

            SqlRepository = new SqlRepository<Identifiable<object>>(SqlDataStore.Object, SqlBuilder.Object, BoFactory.Object, Context.Object);

            UnitOfWork.Setup(_ => _.SqlDataStore).Returns(SqlDataStore.Object);
        }

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
        public void Find_RepositoryNoLinkedToUnitOfWork_ShouldDoNothing()
        {
            SetupOneResultOneSqlResult();

            SqlRepository.Find(_ => true);
        }

        [TestCase]
        public void Find_RepositoryNoLinkedToUnitOfWork_ShouldCallRepository()
        {
            SetupOneResultOneSqlResult();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;
            BoFactory.Setup(_ => _.FillReferences(UnitOfWork.Object, It.IsAny<Identifiable<object>>()));

            SqlRepository.Find(_ => true);

            BoFactory.Verify();
        }

        private void SetupOneResultOneSqlResult()
        {
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(_ => _.Read())
                  .Returns(true)
                  .Returns(false);
            SqlDataStore.Setup(_ => _.Execute(It.IsAny<ISqlQuery>())).Returns(reader.Object);
        }
    }
}
