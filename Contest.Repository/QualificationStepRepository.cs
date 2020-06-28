using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IQualificationStep>))]
    public class QualificationStepRepository : SqlRepositoryBase<QualificationStep, IQualificationStep> { }
}
