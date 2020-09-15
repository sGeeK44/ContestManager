using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IAddress>))]
    [Export(typeof(IRepository<Address, IAddress>))]
    public class AddressRepository : SqlRepositoryBase<Address, IAddress> { }
}
