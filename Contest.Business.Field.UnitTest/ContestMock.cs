using System;
using System.Collections.Generic;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.Fields.UnitTest
{
    internal class ContestMock : IContest
    {
        public ContestMock()
        {
        }

        public DateTime? BeginningDate { get; set; }

        public IEliminationStepSetting ConsolingEliminationSetting { get; set; }

        public Guid ConsolingEliminationSettingId { get; set; }

        public IPhase ConsolingPhase { get; set; }

        public ushort CountField { get; set; }

        public DateTime DatePlanned { get; set; }

        public IEliminationStepSetting EliminationSetting { get; set; }

        public Guid EliminationSettingId { get; set; }

        public DateTime? EndedDate { get; set; }

        public IList<IField> FieldList { get; set; }

        public IGameSetting GameSetting { get; set; }

        public Guid GameSettingId { get; set; }

        public Guid Id { get; set; }

        public bool IsFinished { get; set; }

        public bool IsStarted { get; set; }

        public uint MaximumPlayerByTeam { get; set; }

        public uint MinimumPlayerByTeam { get; set; }

        public IList<IPhase> PhaseList { get; set; }

        public IPhysicalSetting PhysicalSetting { get; set; }

        public Guid PhysicalSettingId { get; set; }

        public IPhase PrincipalPhase { get; set; }

        public IPhase QualificationPhase { get; set; }

        public IQualificationStepSetting QualificationSetting { get; set; }

        public Guid QualificationSettingId { get; set; }

        public IList<ITeam> TeamList { get; set; }

        public bool WithConsolante { get; set; }

        public bool WithQualificationPhase { get; set; }

        public event ContestEvent ContestStart;
        public event NextPhaseStartedEvent NewPhaseLaunch;

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }

        public void EndContest()
        {
            throw new NotImplementedException();
        }

        public bool IsRegister(ITeam team)
        {
            throw new NotImplementedException();
        }

        public void LaunchNextPhase()
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

        public void Register(ITeam team)
        {
            throw new NotImplementedException();
        }

        public void StartContest()
        {
            throw new NotImplementedException();
        }

        public void UnRegister(ITeam team)
        {
            throw new NotImplementedException();
        }
    }
}