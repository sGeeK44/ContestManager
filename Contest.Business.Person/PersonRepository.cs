using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IPerson>))]
    [Export(typeof(ISqlRepository<IPerson>))]
    public class PersonRepository : SqlRepositoryBase<Person, IPerson> { }
}
