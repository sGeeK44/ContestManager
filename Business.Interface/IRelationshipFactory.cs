using Contest.Core.Repository;

namespace Contest.Business
{
    public interface IRelationshipFactory<TIObj1, TIObj2>
        where TIObj1 : IIdentifiable
        where TIObj2 : IIdentifiable
    {
        /// <summary>
        /// Create a new instance of <see cref="T:Business.IRelationship<TIObj1, TIObj2>"/> with specified param
        /// </summary>
        /// <param name="team1">First team involved in match</param>
        /// <param name="team2">Second team involved in match</param>
        /// <returns>New instance</returns>
        IRelationship<TIObj1, TIObj2> Create(TIObj1 first, TIObj2 second);
    }
}