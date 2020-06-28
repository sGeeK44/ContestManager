using Contest.Domain.Games;

namespace Contest.Domain.Settings
{
    public interface IFieldFactory
    {
        /// <summary>
        ///     Create a new instance of Field with specified param
        /// </summary>
        /// <param name="current">Current contest</param>
        /// <param name="name">Name of new field</param>
        /// <returns>Field's instance</returns>
        IField Create(IContest current, string name);
    }
}