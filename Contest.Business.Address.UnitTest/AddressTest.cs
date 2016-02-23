using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contest.Core.Repository.Sql;
using Moq;
using Contest.Core.Repository;

namespace Contest.Business.UnitTest
{
    [TestClass()]
    public class AddressTest
    {
        [TestMethod()]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var address = new Address();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IAddress>(address)).Verifiable();

            address.PrepareCommit(repoMock.Object);
        }

        [TestMethod()]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var address = new Address();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IAddress>(address)).Verifiable();

            address.PrepareDelete(repoMock.Object);
        }
    }
}