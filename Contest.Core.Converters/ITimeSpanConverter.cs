using System;

namespace Contest.Core.Converters
{
    public interface ITimeSpanConverter
    {
        /// <summary>
        /// Convert TimeSpan in .NET representation to equivalent Date object representation in target system
        /// </summary>
        /// <param name="value">A TimeSpan in .NET</param>
        /// <returns>A Date object in target system</returns>
        string ToString(TimeSpan value);

        /// <summary>
        /// Convert Time object for target system to equivalent TimeSpan in .NET representation
        /// </summary>
        /// <param name="value">A Time object in target system</param>
        /// <returns>A TimeSpan in .NET</returns>
        TimeSpan ToTimeSpan(string value);
    }
}
