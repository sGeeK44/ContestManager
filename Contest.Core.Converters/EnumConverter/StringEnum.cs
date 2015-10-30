using System;

namespace Contest.Core.Converters.EnumConverter
{
    public abstract class StringEnum : IEnumConverter
    {
        #region Constructors

        protected StringEnum() { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert Enum in .NET representation to equivalent Enum object representation in target system
        /// </summary>
        /// <param name="value">A Enum in .NET</param>
        /// <returns>A Enum object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public abstract string ToString(Enum value);

        /// <summary>
        /// Convert Enum object for String system to equivalent Enum in .NET representation
        /// </summary>
        /// <param name="value">A Enum object in String system</param>
        /// <param name="enumType">Target Type of enum</param>
        /// <returns>A Enum in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public abstract Enum ToEnum(string value, Type enumType);

        #endregion
    }
}
