﻿using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.UnitTest.Kit;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatch>))]
    public class MatchRepositoryMock : RepositoryBaseMock<IMatch> { }
}
