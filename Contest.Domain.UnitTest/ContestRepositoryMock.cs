﻿using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<Domain.Games.Contest, IContest>))]
    public class ContestRepositoryMock : RepositoryBaseMock<Domain.Games.Contest, IContest>
    {
    }
}
