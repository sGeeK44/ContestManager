using System;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Moq;

namespace Contest.Business.UnitTest
{
    public static class Helper
    {
        public static Mock<T> CreateMock<T>(string mockId) where T : class, IIdentifiable, IQueryable
        {
            if (mockId == null) return null;

            var factory = new MockRepository(MockBehavior.Default);
            var result = factory.Create<T>();
            result.SetupGet(_ => _.Id).Returns(new Guid(mockId));
            return result;
        }

        public static T GetMock<T>(Mock<T> mock) where T : class
        {
            return mock == null ? null : mock.Object;
        }

        internal static IMatch CreateMatch(string gameStepId, string team1Id, string team2Id, string matchSettingId)
        {
            return CreateMatch(gameStepId, team1Id, team2Id, matchSettingId, false, EndTypeConstaint.Point, null, 13);
        }

        internal static IMatch CreateMatch(string gameStepId, string team1Id, string team2Id, string matchSettingId, bool canBeDuce, EndTypeConstaint endBy, TimeSpan? elapse, ushort matchPoint)
        {
            var matchSetting = CreateMock<IMatchSetting>(matchSettingId);
            matchSetting.SetupGet(_ => _.Id).Returns(new Guid(matchSettingId));
            matchSetting.SetupGet(_ => _.CanBeDuce).Returns(canBeDuce);
            matchSetting.SetupGet(_ => _.EndBy).Returns(endBy);
            matchSetting.SetupGet(_ => _.MatchPoint).Returns(matchPoint);
            string mess;
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            string message;
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out message)).Returns(true);
            return CreateMatch(gameStepId, team1Id, team2Id, matchSetting.Object);
        }

        internal static IMatch CreateMatch(string gameStepId, string team1Id, string team2Id, IMatchSetting matchSetting)
        {
            var gameStep = CreateMock<IGameStep>(gameStepId);
            var team1 = CreateMock<ITeam>(team1Id);
            var team2 = CreateMock<ITeam>(team2Id);
            return new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting);
        }
    }
}
