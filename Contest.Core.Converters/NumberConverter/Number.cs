using System;
using System.Globalization;

namespace Contest.Core.Converters.NumberConverter
{
    public abstract class Number : INumberConverter
    {
        /// <summary>
        /// Represent format for Number
        /// </summary>
        public abstract string Pattern { get; }

        #region Constructors

        protected Number() { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert ushort integer in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A ushort integer in .NET</param>
        /// <returns>A ushort integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(ushort value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter ushort into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter ushort into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert ushort interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A ushort interger object in String system</param>
        /// <returns>An ushort integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public ushort ToUShort(string value)
        {
            try { return ushort.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into ushort in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert short integer in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A short integer in .NET</param>
        /// <returns>A short integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(short value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter short into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter short into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert short interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A short interger object in String system</param>
        /// <returns>An short integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public short ToShort(string value)
        {
            try { return short.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into short in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert uinterger in .NET representation to equivalent uinteger object representation in target system
        /// </summary>
        /// <param name="value">A uinteger in .NET</param>
        /// <returns>A uinteger object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(uint value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter uint into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter uint into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert uinterger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A uinterger object in String system</param>
        /// <returns>An uinteger in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public uint ToUInteger(string value)
        {
            try { return uint.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into uint in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert interger in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A integer in .NET</param>
        /// <returns>A integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(int value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter int into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter int into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A interger object in String system</param>
        /// <returns>An integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public int ToInteger(string value)
        {
            try { return int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into int in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert ulong in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A ulong in .NET</param>
        /// <returns>A ulong object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(ulong value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter ulong into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter ulong into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert ulong object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A ulong object in String system</param>
        /// <returns>An ulong in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public ulong ToULong(string value)
        {
            try { return ulong.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into ulong in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert long in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A long in .NET</param>
        /// <returns>A long object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(long value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter long into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter long into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert long object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A long object in String system</param>
        /// <returns>An long in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public long ToLong(string value)
        {
            try { return long.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into long in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert double in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A double in .NET</param>
        /// <returns>A double object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(double value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter double into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter double into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert double object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A double object in String system</param>
        /// <returns>A double in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public double ToDouble(string value)
        {
            try { return double.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into double in that specified format. Value: {0}", value), exception);
            }
        }

        /// <summary>
        /// Convert float in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A float in .NET</param>
        /// <returns>A float object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(float value)
        {
            if (string.IsNullOrWhiteSpace(Pattern))
            {
                try { return value.ToString(CultureInfo.InvariantCulture); }
                catch (Exception exception)
                {
                    throw new ConverterException(string.Format("Failed to converter float into string in that specified format. Value: {0}", value), exception);
                }
            }
            try { return value.ToString(Pattern, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter float into string in that specified format. Value: {0}, Pattern:{1}", value, Pattern), exception);
            }
        }

        /// <summary>
        /// Convert float object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A float object in String system</param>
        /// <returns>A float in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public float ToFloat(string value)
        {
            try { return float.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture); }
            catch (Exception exception)
            {
                throw new ConverterException(string.Format("Failed to converter string value into float in that specified format. Value: {0}", value), exception);
            }
        }

        #endregion
    }
}
