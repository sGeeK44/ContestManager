using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Repository
{
    [Export(typeof(IRepository<Field, IField>))]
    public class FieldRepository : SqlRepositoryBase<Field, IField> { }
}
