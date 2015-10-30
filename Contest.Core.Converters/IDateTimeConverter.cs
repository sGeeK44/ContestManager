using System;

namespace Contest.Core.Converters
{
    public interface IDateTimeConverter
    {
        /// <summary>
        /// Convert DateTime in .NET representation to equivalent Date object representation in target system
        /// </summary>
        /// <param name="value">A DateTime in .NET</param>
        /// <returns>A Date object in target system</returns>
        string ToString(DateTime value);

        /// <summary>
        /// Convert Date object for target system to equivalent DateTime in .NET representation
        /// </summary>
        /// <param name="value">A Date object in target system</param>
        /// <returns>A DateTime in .NET</returns>
        DateTime ToDateTime(string value);
    }
}
