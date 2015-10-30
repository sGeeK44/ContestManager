using System;

namespace Contest.Core.Converters
{
    public interface IGuidConverter
    {
        /// <summary>
        /// Convert Guid in .NET representation to equivalent Guid object representation in target system
        /// </summary>
        /// <param name="value">A Guid in .NET</param>
        /// <returns>A Guid object in target system</returns>
        string ToString(Guid value);

        /// <summary>
        /// Convert Guid object for target system to equivalent Guid in .NET representation
        /// </summary>
        /// <param name="value">A Guid object in target system</param>
        /// <returns>A Guid in .NET</returns>
        Guid ToGuid(string value);
    }
}
