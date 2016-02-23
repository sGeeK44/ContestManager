using System;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.Fields.UnitTest
{
    internal class MatchMock : IMatch
    {
        public MatchMock()
        {
        }

        public DateTime? Beginning { get; set; }

        public TimeSpan? Elapse { get; set; }

        public DateTime? Endded { get; set; }

        public IGameStep GameStep { get; set; }

        public Guid GameStepId { get; set; }

        public Guid Id { get; set; }

        public bool IsBeginning { get; set; }

        public bool IsClose { get; set; }

        public bool IsFinished { get; set; }

        public IField MatchField { get; set; }

        public MatchState MatchState { get; set; }

        public IMatchSetting Setting { get; set; }

        public ITeam Team1 { get; set; }

        public Guid Team1Id { get; set; }

        public ITeam Team2 { get; set; }

        public Guid Team2Id { get; set; }

        public ushort TeamScore1 { get; set; }

        public ushort TeamScore2 { get; set; }

        public ITeam Winner { get; set; }

        public event MatchEvent MatchClosed;
        public event MatchEvent MatchEnded;
        public event MatchEvent MatchStarted;
        public event MatchEvent ScoreChanged;

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Equals(IIdentifiable other)
        {
            throw new NotImplementedException();
        }

        public bool IsTeamInvolved(ITeam team)
        {
            throw new NotImplementedException();
        }

        public void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        public void SetResult(ushort teamScore1, ushort teamScore2)
        {
            throw new NotImplementedException();
        }

        public void Start(IField field)
        {
            throw new NotImplementedException();
        }

        public void UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            throw new NotImplementedException();
        }
    }
}