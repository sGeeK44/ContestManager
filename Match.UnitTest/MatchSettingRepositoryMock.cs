﻿using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatchSetting>))]
    [Export(typeof(ISqlRepository<IMatchSetting>))]
    public class MatchSettingRepositoryMock : RepositoryMockBase<IMatchSetting>, ISqlRepository<IMatchSetting>
    {
    }
}