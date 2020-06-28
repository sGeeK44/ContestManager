using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IQualificationStep>))]
    public class QualificationStepRepositoryMock : RepositoryBaseMock<IQualificationStep>
    {
    }
}