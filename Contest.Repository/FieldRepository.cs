using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IField>))]
    [Export(typeof(IRepository<Field, IField>))]
    public class FieldRepository : SqlRepositoryBase<Field, IField> { }
}
