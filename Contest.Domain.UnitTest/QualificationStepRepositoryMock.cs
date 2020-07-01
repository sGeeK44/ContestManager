using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<QualificationStep, IQualificationStep>))]
    public class QualificationStepRepositoryMock : RepositoryBaseMock<QualificationStep, IQualificationStep>
    {
    }
}