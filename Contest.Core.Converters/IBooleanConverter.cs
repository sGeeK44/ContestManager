namespace Contest.Core.Converters
{
    public interface IBooleanConverter
    {
        /// <summary>
        /// Convert boolean in .NET representation to equivalent boolean object representation in target system
        /// </summary>
        /// <param name="value">A boolean in .NET</param>
        /// <returns>A boolean object in target system</returns>
        string ToString(bool value);

        /// <summary>
        /// Convert boolean object for target system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A boolean object in target system</param>
        /// <returns>A boolean  in .NET</returns>
        bool ToBoolean(string value);
    }
}
