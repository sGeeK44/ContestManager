using Contest.Domain.Matchs;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class MatchSettingTest
    {
        [TestCase]
        public void DefaultValue()
        {
            var match = new MatchSetting();

            Assert.IsFalse(match.CanBeDuce);
            Assert.AreEqual(EndTypeConstaint.Point, match.EndBy);
            Assert.IsNotNull(match.Id);
            Assert.AreEqual(13, match.MatchPoint);
            Assert.AreEqual(0, match.PointForDuce);
            Assert.AreEqual(0, match.PointForLoose);
            Assert.AreEqual(1, match.PointForWin);
        }

        [TestCase(false, 13, 0, 0, 0)]
        [TestCase(true, 13, 3, 2, 1)]
        public void Create_MatchEndByPoint_1(bool canBeDuce, int matchPoint, int pointForWin, int pointForLoose, int pointForDuce)
        {

            var match = new MatchSetting(canBeDuce, (ushort)matchPoint, (ushort)pointForWin, (ushort)pointForLoose, (ushort)pointForDuce);

            Assert.AreEqual(canBeDuce, match.CanBeDuce);
            Assert.AreEqual(EndTypeConstaint.Point, match.EndBy);
            Assert.IsNotNull(match.Id);
            Assert.AreEqual(matchPoint, match.MatchPoint);
            Assert.AreEqual(pointForDuce, match.PointForDuce);
            Assert.AreEqual(pointForLoose, match.PointForLoose);
            Assert.AreEqual(pointForWin, match.PointForWin);
        }

        [TestCase(0, 0, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        [TestCase(2, 1, true)]
        [TestCase(2, 3, false)]
        public void IsValidResult(int matchPoint, int teamScore, bool expectedIsValid)
        {
            var match = new MatchSetting(true, (ushort)matchPoint, 0, 0, 0);
            string message;
            Assert.AreEqual(expectedIsValid, match.IsValidScore((ushort)teamScore, out message));
        }

        [TestCase(true, 0, 0, 0, true)]
        [TestCase(false, 0, 0, 0, false)]
        [TestCase(true, 13, 0, 0, false)]
        [TestCase(false, 13, 0, 0, false)]
        [TestCase(true, 13, 13, 0, true)]
        [TestCase(true, 13, 0, 13, true)]
        [TestCase(false, 13, 13, 13, false)]
        [TestCase(false, 13, 14, 13, false)]
        [TestCase(false, 13, 13, 14, false)]
        public void IsValidToFinishedResult(bool canBeDuce, int matchPoint, int teamScore1, int teamScore2, bool expectedIsValid)
        {
            var match = new MatchSetting(canBeDuce, (ushort)matchPoint, 0, 0, 0);
            string message;
            Assert.AreEqual(expectedIsValid, match.IsValidToFinishedMatch((ushort)teamScore1, (ushort)teamScore2, out message));
        }
    }
}
