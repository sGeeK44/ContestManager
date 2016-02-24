using NUnit.Framework;
using System;
using Contest.Core.Repository.Sql;
using Moq;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class GameSettingTest
    {

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new GameSetting();

            Assert.Throws<ArgumentNullException>(() => field.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var field = new GameSetting();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IGameSetting>(field)).Verifiable();

            field.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new GameSetting();

            Assert.Throws<ArgumentNullException>(() => field.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var field = new GameSetting();

            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IGameSetting>(field)).Verifiable();

            field.PrepareDelete(repoMock.Object);
        }
    }
}