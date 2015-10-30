using System;
using System.Globalization;

namespace Contest.Core.Converters.TimeSpanConverter
{
    public abstract class StringTimeSpan : ITimeSpanConverter
    {
        /// <summary>
        /// Represent format for TimeSpan ex:'hhmmss'
        /// </summary>
        public abstract string Pattern { get; }

        #region Constructors

        protected StringTimeSpan()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert TimeSpan in .NET representation to equivalent Date object representation in target system
        /// </summary>
        /// <param name="value">A TimeSpan in .NET</param>
        /// <returns>A Date object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(TimeSpan value)
        {
            try { return string.Format(string.Concat("{0:", Pattern, "}"), value); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter TimeSpan into string in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert Time object for String system to equivalent TimeSpan in .NET representation
        /// </summary>
        /// <param name="value">A Time object in String system</param>
        /// <returns>A TimeSpan in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public TimeSpan ToTimeSpan(string value)
        {
            try { return TimeSpan.ParseExact(value, Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into TimeSpan in that specified format. Value: {0}", value), exception);
            }
        }

        #endregion
    }
}
