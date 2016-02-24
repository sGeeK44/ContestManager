using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IQualificationStep>))]
    [Export(typeof(ISqlRepository<IQualificationStep>))]
    public class QualificationStepRepository : SqlRepository<QualificationStep, IQualificationStep> { }
}
