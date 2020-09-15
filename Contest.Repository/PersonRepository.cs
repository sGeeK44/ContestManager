using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IPerson>))]
    [Export(typeof(IRepository<Person, IPerson>))]
    public class PersonRepository : SqlRepositoryBase<Person, IPerson> { }
}
