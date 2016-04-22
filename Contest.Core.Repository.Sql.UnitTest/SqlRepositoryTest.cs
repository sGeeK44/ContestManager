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
        public Mock<ISqlDataStore> UnitOfWorkDataStore { get; set; }
        public Mock<IBusinessObjectFactory<Identifiable<object>>> BoFactory { get; set; }
        public Mock<IDataReader> Reader { get; set; }
        public SqlRepository<Identifiable<object>> SqlRepository { get; set; }

        [SetUp]
        public void Init()
        {
            Context = new Mock<IDataContext<Identifiable<object>>>();
            SqlBuilder = new Mock<ISqlQueryFactory<Identifiable<object>>>();
            UnitOfWork = new Mock<ISqlUnitOfWorks>();
            SqlDataStore = new Mock<ISqlDataStore>();
            BoFactory = new Mock<IBusinessObjectFactory<Identifiable<object>>>();
            Reader = new Mock<IDataReader>();
            UnitOfWorkDataStore = new Mock<ISqlDataStore>();

            SqlRepository = new SqlRepository<Identifiable<object>>(SqlDataStore.Object, SqlBuilder.Object, BoFactory.Object, Context.Object);

            UnitOfWork.Setup(_ => _.SqlDataStore).Returns(UnitOfWorkDataStore.Object);
        }
        
        [TestCase]
        public void Insert_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Insert(obj));

            SqlRepository.Insert(obj);

            Context.VerifyAll();
        }

        [TestCase]
        public void Insert_ValidObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Insert(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void Insert_WithUnitOfWork_InnerStatementShouldBeUpdate()
        {
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Insert(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Insert_WithUnitOfWork_UnitOfWorkDataStoreShouldBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Insert(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Insert(obj));

            SqlRepository.InsertOrUpdate(obj);

            Context.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.InsertOrUpdate(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_AlreadyPresentObject_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);
            Context.Setup(_ => _.Update(obj));

            SqlRepository.InsertOrUpdate(obj);

            Context.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_AlreadyPresentObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);

            SqlRepository.InsertOrUpdate(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_InnerStatementShouldBeUpdate()
        {
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.InsertOrUpdate(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void InsertOrUpdate_WithUnitOfWork_UnitOfWorkDataStoreShouldBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.InsertOrUpdate(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Update_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Update(obj));

            SqlRepository.Update(obj);

            Context.VerifyAll();
        }

        [TestCase]
        public void Update_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Update(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void Update_WithUnitOfWork_InnerStatementShouldBeUpdate()
        {
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Update(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Update_WithUnitOfWork_UnitOfWorkDataStoreShouldBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Update(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Delete_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();

            SqlRepository.Delete(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void Delete_AlreadyPresentObject_ContextShouldBeUpdated()
        {
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.Delete(obj));

            SqlRepository.Delete(obj);

            Context.VerifyAll();
        }

        [TestCase]
        public void Delete_AlreadyPresentObject_QueryListShouldContainNewStatement()
        {
            SqlDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            Context.Setup(_ => _.IsExist(obj)).Returns(true);

            SqlRepository.Delete(obj);

            SqlDataStore.VerifyAll();
        }

        [TestCase]
        public void Delete_WithUnitOfWork_InnerStatementShouldBeUpdate()
        {
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            var obj = new Identifiable<object>();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Delete(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Delete_WithUnitOfWork_UnitOfWorkDataStoreShouldBeUpdate()
        {
            var obj = new Identifiable<object>();
            UnitOfWorkDataStore.Setup(_ => _.AddRequest(It.IsAny<ISqlQuery>()));
            SqlRepository.UnitOfWorks = UnitOfWork.Object;

            SqlRepository.Delete(obj);

            UnitOfWorkDataStore.VerifyAll();
        }

        [TestCase]
        public void Find_RepositoryNoLinkedToUnitOfWork_ShouldDoNothing()
        {
            SetupOneResultOneSqlResult();

            SqlRepository.Find(_ => true);
        }

        [TestCase]
        public void Find_RepositoryNoLinkedToUnitOfWork_ShouldCallBoFactory()
        {
            var result = new Mock<Identifiable<object>>();
            SetupOneResultOneSqlResult();
            SqlRepository.UnitOfWorks = UnitOfWork.Object;
            BoFactory.Setup(_ => _.Convert(Reader.Object)).Returns(result.Object);
            BoFactory.Setup(_ => _.FillReferences(UnitOfWork.Object, result.Object));

            SqlRepository.Find(_ => true);

            BoFactory.VerifyAll();
        }

        private void SetupOneResultOneSqlResult()
        {
            Reader.SetupSequence(_ => _.Read())
                  .Returns(true)
                  .Returns(false);
            SqlDataStore.Setup(_ => _.Execute(It.IsAny<ISqlQuery>())).Returns(Reader.Object);
        }
    }
}
