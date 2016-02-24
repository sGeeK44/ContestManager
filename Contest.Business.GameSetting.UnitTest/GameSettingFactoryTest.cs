using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Contest.Business.UnitTest
{
    [TestClass()]
    public class GameSettingFactoryTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_WithZeroMinimumPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            var result = factory.Create(0, 10);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_WithZeroMaximumPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            var result = factory.Create(10, 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_WithLessMaxThanMinPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            var result = factory.Create(20, 10);
        }

        [TestMethod()]
        public void Create_WithSameMaxMinPlayer_ShouldInitializeAnIGameSetting()
        {
            var expectedResult = CreateExpectedResult(10, 10);
            var factory = new GameSettingFactory();

            var result = factory.Create(expectedResult.MinimumPlayerByTeam, expectedResult.MaximumPlayerByTeam);

            Assert.AreEqual(expectedResult, result);
        }

        private IGameSetting CreateExpectedResult(uint minPlayer, uint maxPlayer)
        {
            var expectedResult = new GameSettingStub();
            expectedResult.MinimumPlayerByTeam = minPlayer;
            expectedResult.MaximumPlayerByTeam = maxPlayer;
            return expectedResult;
        }

        [TestMethod()]
        public void Create_WithValidPlayerNumber_ShouldInitializeAnIGameSetting()
        {
            var expectedResult = CreateExpectedResult(1, 3);
            var factory = new GameSettingFactory();

            var result = factory.Create(expectedResult.MinimumPlayerByTeam, expectedResult.MaximumPlayerByTeam);

            Assert.AreEqual(expectedResult, result);
        }
    }
}