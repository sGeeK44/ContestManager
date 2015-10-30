using System;
using System.Globalization;

namespace Contest.Core.Converters.DateTimeConverter
{
    public abstract class StringDateTime : IDateTimeConverter
    {
        /// <summary>
        /// Represent format for DateTime ex:'yyyyMMdd'
        /// </summary>
        public abstract string Pattern { get; }

        #region Constructors

        protected StringDateTime()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert DateTime in .NET representation to equivalent Date object representation in target system
        /// </summary>
        /// <param name="value">A DateTime in .NET</param>
        /// <returns>A Date object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(DateTime value)
        {
            try { return string.Format(string.Concat("{0:", Pattern, "}"), value); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter DateTime into string in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert Date object for String system to equivalent DateTime in .NET representation
        /// </summary>
        /// <param name="value">A Date object in String system</param>
        /// <returns>A DateTime in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public DateTime ToDateTime(string value)
        {
            try { return DateTime.ParseExact(value, Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into DateTime in that specified format. Value: {0}", value), exception);
            }
        }

        #endregion
    }
}
