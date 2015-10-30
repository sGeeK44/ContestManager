using System;
using Contest.Core.Exceptions;

namespace Contest.Core.Converters.EnumConverter
{
    /// <summary>
    /// Attribut to associate String string value for an Enum field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StringValueAttribute : Attribute
    {
        /// <summary>
        /// Associated String value for an enum field
        /// </summary>
        public string AssociatedValue { get; private set; }

        /// <summary>
        /// Initialize a new StringValueAttribute with specified String string value
        /// </summary>
        /// <param name="value">String string value associated to this enum field</param>
        public StringValueAttribute(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            AssociatedValue = value;
        }

        #region Static Methods

        /// <summary>
        /// Convert the String string value representation to its enumeration value equivalent.
        /// </summary>
        /// <param name="t">Type of enum target</param>
        /// <param name="s">A string to convert.</param>
        /// <param name="ignoreCase"> A System.Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="result">When this methods return, contains the first Enum representation if the conversion succeeded, the enum value equal null if conversion failed</param>
        /// <returns>true if conversion succeed, false else</returns>
        static public bool TryParse(Type t, string s, bool ignoreCase, out Enum result)
        {
            result = null;
            try
            {
                result = Parse(t, s, ignoreCase);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Converts the string String Value representation to its enumeration value equivalent.
        /// </summary>
        /// <param name="t">Type of enum to convert</param>
        /// <param name="s">A string to convert.</param>
        /// <param name="ignoreCase"> A System.Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <returns>Enumeration value equivalent</returns>
        /// <exception cref="System.NotSupportedException"/>
        public static Enum Parse(Type t, string s, bool ignoreCase)
        {
            if (s == null) throw new ArgumentNullException("s");
            if (t == null) throw new ArgumentNullException("t");

            var correspondingValue = 0;
            if (t.IsEnum)
            {
                Enum result = null;
                foreach (Enum enumValue in Enum.GetValues(t))
                {
                    try
                    {
                        var tmcValue = enumValue.GetStringValue();
                        if (string.Compare(tmcValue, s, ignoreCase) != 0) continue;
                        result = enumValue;
                        correspondingValue++;
                    }
                    catch { }
                }
                if (correspondingValue == 0) throw new ArgumentException(string.Format("None Enum field correspond to that {0}'s StringValue value: {1}", t, s), "s");
                if (correspondingValue > 1) throw new NotSupportedException(string.Format("Several Enum field correspond to that {0}'s StringValue value: {1}", t, s));
                return result;
            }
            throw new ArgumentException("StringValueAttribute is only design for Enum Field or CustomStruct");
        }

        #endregion
    }

    public static class StringValueEnumExtension
    {
        /// <summary>
        /// Give associated StringValueAttribute of an enum who had specify it in its attributs
        /// </summary>
        /// <param name="value">Enum to translate</param>
        /// <param name="result">String value corresponding if conversion succeed, null else</param>
        /// <returns>Value of attribut founded</returns>
        /// <exception cref="System.NotSupportedException">Throw when current value enum haven't specified StringValue or had specified more than one in their attributs.</exception>
        public static bool TryGetStringValue(this Enum value, out string result)
        {
            try
            {
                result = value.GetStringValue();
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Give associated StringValueAttribute of an enum who had specify it in its attributs
        /// </summary>
        /// <param name="value">Enum to translate</param>
        /// <returns>Value of attribut founded</returns>
        /// <exception cref="System.NotSupportedException">Throw when current value enum haven't specified StringValue or had specified more than one in their attributs.</exception>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            if (!Enum.IsDefined(type, value)) throw new NotSupportedException("Enum value is not defined.");

            // Get fieldinfo for this type
            var field = type.GetField(value.ToString());
            if (field == null) throw new NotSupportedException("Unable to get FieldInfo for this enum.");

            // Get the stringvalue attributes
            var attribs = field.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            if (attribs == null || attribs.Length == 0) throw new NotFoundException(typeof(StringValueAttribute));
            if (attribs.Length > 1) throw new SeveralFoundException(typeof(StringValueAttribute), attribs.Length);
            return attribs[0].AssociatedValue;
        }
    }
}
