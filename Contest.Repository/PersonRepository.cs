using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;

namespace Contest.Repository
{
    [Export(typeof(IRepository<Person, IPerson>))]
    public class PersonRepository : SqlRepositoryBase<Person, IPerson> { }
}
