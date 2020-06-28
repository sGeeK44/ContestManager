using System;
using Contest.Domain.Games;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class GameSettingFactoryTest
    {
        [TestCase]
        public void Create_WithZeroMinimumPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            Assert.Throws<ArgumentException>(() => factory.Create(0, 10));
        }

        [TestCase]
        public void Create_WithZeroMaximumPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            Assert.Throws<ArgumentException>(() => factory.Create(10, 0));
        }

        [TestCase]
        public void Create_WithLessMaxThanMinPlayer_ShouldThrowArgumentException()
        {
            var factory = new GameSettingFactory();

            Assert.Throws<ArgumentException>(() => factory.Create(20, 10));
        }

        [TestCase(10, 10)]
        [TestCase(1, 3)]
        public void Create_WithValid_ShouldInitializeAnIGameSetting(int min, int max)
        {
            var factory = new GameSettingFactory();

            var result = factory.Create((uint)min, (uint)max);

            Assert.AreEqual(min, result.MinimumPlayerByTeam);
            Assert.AreEqual(max, result.MaximumPlayerByTeam);
        }
    }
}