using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IAddress>))]
    public class AddressRepository : SqlRepositoryBase<Address, IAddress> { }
}
