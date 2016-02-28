namespace Contest.Core.Repository.Sql.UnitTest
{
    public interface IEntity5 : IId, IQueryable
    {
        string Name { get; set; }

        bool Active { get; set; }

        int Age { get; set; }
    }
}