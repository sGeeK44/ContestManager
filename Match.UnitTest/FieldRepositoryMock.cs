using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IField>))]
    [Export(typeof(ISqlRepository<IField>))]
    public class FieldRepositoryMock : RepositoryMockBase<IField>, ISqlRepository<IField>
    {
    }
}