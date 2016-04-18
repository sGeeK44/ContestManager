using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IAddress>))]
    [Export(typeof(ISqlRepository<IAddress>))]
    public class AddressRepository : SqlRepositoryBase<Address, IAddress> { }
}
