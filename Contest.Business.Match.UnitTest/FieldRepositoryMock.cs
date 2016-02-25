using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IField>))]
    [Export(typeof(ISqlRepository<IField>))]
    public class FieldRepositoryMock : SqlRepositoryBaseMock<IField>, ISqlRepository<IField>
    {
    }
}