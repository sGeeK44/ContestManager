using System;

namespace Contest.Core.Converters
{
    public interface IEnumConverter
    {
        /// <summary>
        /// Convert Enum in .NET representation to equivalent Enum object representation in target system
        /// </summary>
        /// <param name="value">A Enum in .NET</param>
        /// <returns>A Enum object in target system</returns>
        string ToString(Enum value);

        /// <summary>
        /// Convert Enum object for target system to equivalent Enum in .NET representation
        /// </summary>
        /// <param name="value">A Enum object in target system</param>
        /// <param name="enumType">TypeOf enum target</param>
        /// <returns>A Enum in .NET</returns>
        Enum ToEnum(string value, Type enumType);
    }
}
