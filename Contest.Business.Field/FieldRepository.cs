using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IField>))]
    [Export(typeof(ISqlRepository<IField>))]
    public class FieldRepository : SqlRepository<Field, IField> { }
}
