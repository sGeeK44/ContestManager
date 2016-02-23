using System;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.UnitTest
{
    internal class GameSettingStub : IGameSetting
    {
        public Guid Id { get; set; }

        public uint MaximumPlayerByTeam { get; set; }

        public uint MinimumPlayerByTeam { get; set; }

        public bool AreSame(object other)
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
    }
}