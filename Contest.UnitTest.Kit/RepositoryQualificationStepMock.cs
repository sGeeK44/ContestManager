using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IQualificationStep>))]
    public class RepositoryQualificationStepMock : RepositoryBaseMock<IQualificationStep>
    { }
}