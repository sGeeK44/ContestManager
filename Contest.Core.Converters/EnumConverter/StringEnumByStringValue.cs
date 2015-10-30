using System;
using Contest.Core.Exceptions;

namespace Contest.Core.Converters.EnumConverter
{
    public class StringEnumByStringValue : StringEnum
    {
        #region Methods

        /// <summary>
        /// Convert Enum in .NET representation to equivalent Enum object representation in target system
        /// </summary>
        /// <param name="value">A Enum in .NET</param>
        /// <returns>A Enum object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public override string ToString(Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            try { return value.GetStringValue(); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter Enum into string in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert Enum object for String system to equivalent Enum in .NET representation
        /// </summary>
        /// <param name="value">A Enum object in String system</param>
        /// <param name="enumType">Target Type of enum</param>
        /// <returns>A Enum in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public override Enum ToEnum(string value, Type enumType)
        {
            if (value == null) value = string.Empty;
            if (enumType == null) throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum) throw new InvalidTypeException(typeof(Enum), enumType);

            try { return StringValueAttribute.Parse(enumType, value, false); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into Enum ({0}) in that specified format. Value: {1}", enumType, value), exception);
            }
        }

        #endregion
    }
}
