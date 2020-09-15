using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IField>))]
    [Export(typeof(IRepository<Field, IField>))]
    public class FieldRepositoryMock : RepositoryBaseMock<Field, IField>
    {
    }
}