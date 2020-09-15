using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IQualificationStep>))]
    [Export(typeof(IRepository<QualificationStep, IQualificationStep>))]
    public class QualificationStepRepositoryMock : RepositoryBaseMock<QualificationStep, IQualificationStep>
    {
    }
}