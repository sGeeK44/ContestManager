using System;

namespace Contest.Core.Converters.GuidConverter
{
    public abstract class StringGuid : IGuidConverter
    {
        /// <summary>
        /// Represent format for Guid
        /// </summary>
        public abstract string Pattern { get; }

        #region Constructors

        protected StringGuid()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert Guid in .NET representation to equivalent Date object representation in target system
        /// </summary>
        /// <param name="value">A Guid in .NET</param>
        /// <returns>A Date object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(Guid value)
        {
            try { return Pattern != null ? string.Format(string.Concat("{0:", Pattern, "}"), value).ToUpper() : value.ToString().ToUpper(); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter Guid into string in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert Date object for String system to equivalent Guid in .NET representation
        /// </summary>
        /// <param name="value">A Date object in String system</param>
        /// <returns>A Guid in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public Guid ToGuid(string value)
        {
            try { return Pattern != null ? Guid.ParseExact(value, Pattern) : Guid.Parse(value); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into Guid in that specified format. Value: {0}", value), exception);
            }
        }

        #endregion
    }
}
