namespace Contest.Core.Converters
{
    public interface INumberConverter
    {


        /// <summary>
        /// Convert ushort integer in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A ushort integer in .NET</param>
        /// <returns>A ushort integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(ushort value);

        /// <summary>
        /// Convert ushort interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A ushort interger object in String system</param>
        /// <returns>An ushort integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        ushort ToUShort(string value);

        /// <summary>
        /// Convert short integer in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A short integer in .NET</param>
        /// <returns>A short integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(short value);

        /// <summary>
        /// Convert short interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A short interger object in String system</param>
        /// <returns>An short integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        short ToShort(string value);

        /// <summary>
        /// Convert uinterger in .NET representation to equivalent uinteger object representation in target system
        /// </summary>
        /// <param name="value">A uinteger in .NET</param>
        /// <returns>A uinteger object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(uint value);

        /// <summary>
        /// Convert uinterger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A uinterger object in String system</param>
        /// <returns>An uinteger in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        uint ToUInteger(string value);

        /// <summary>
        /// Convert interger in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A integer in .NET</param>
        /// <returns>A integer object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(int value);

        /// <summary>
        /// Convert interger object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A interger object in String system</param>
        /// <returns>An integer in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        int ToInteger(string value);

        /// <summary>
        /// Convert ulong in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A ulong in .NET</param>
        /// <returns>A ulong object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(ulong value);

        /// <summary>
        /// Convert ulong object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A ulong object in String system</param>
        /// <returns>An ulong in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        ulong ToULong(string value);

        /// <summary>
        /// Convert long in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A long in .NET</param>
        /// <returns>A long object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(long value);

        /// <summary>
        /// Convert long object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A long object in String system</param>
        /// <returns>An long in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        long ToLong(string value);

        /// <summary>
        /// Convert double in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A double in .NET</param>
        /// <returns>A double object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(double value);

        /// <summary>
        /// Convert double object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A double object in String system</param>
        /// <returns>A double in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        double ToDouble(string value);

        /// <summary>
        /// Convert float in .NET representation to equivalent integer object representation in target system
        /// </summary>
        /// <param name="value">A float in .NET</param>
        /// <returns>A float object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        string ToString(float value);

        /// <summary>
        /// Convert float object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A float object in String system</param>
        /// <returns>A float in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        float ToFloat(string value);
    }
}