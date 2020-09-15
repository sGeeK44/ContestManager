using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IQualificationStep>))]
    [Export(typeof(IRepository<QualificationStep, IQualificationStep>))]
    public class QualificationStepRepository : SqlRepositoryBase<QualificationStep, IQualificationStep> { }
}
