using System;
using System.Diagnostics.Contracts;
using Contest.Core.Exceptions;

namespace Contest.Core.Windows
{
    /// <summary>
    /// This attribut provide methods to associated an localized label for an enum field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAttribute : Attribute
    {
        /// <summary>
        /// Get associated label
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Associate a globalized Label for an enum field
        /// </summary>
        /// <param name="label">Set the associated label</param>
        public DisplayAttribute(string label)
        {
            Value = label ?? throw new ArgumentNullException(nameof(label));
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
            Contract.Requires(t != null);
            Contract.Requires(s != null);
            Contract.Requires(t.IsEnum);

            if (s == null) throw new ArgumentNullException(nameof(s));
            if (t == null) throw new ArgumentNullException(nameof(t));
            if (!t.IsEnum) throw new ArgumentException("DisplayAttribute is only design for Enum Field");

            Enum result = null;
            var correspondingValue = 0;
            foreach (Enum enumValue in Enum.GetValues(t))
            {
                try
                {
                    var tmcValue = enumValue.Display();
                    if (string.Compare(tmcValue, s, ignoreCase) != 0) continue;
                    result = enumValue;
                    correspondingValue++;
                }
                catch { }
            }
            if (correspondingValue == 0) throw new ArgumentException(
                $"None Enum field correspond to that {t}'s StringValue value: {s}", nameof(s));
            if (correspondingValue > 1) throw new NotSupportedException(
                $"Several Enum field correspond to that {t}'s StringValue value: {s}");
            return result;
        }
    }

    /// <summary>
    /// Provide helper method on enum to manipulate DisplayAttribute
    /// </summary>
    public static class DisplayAttributeEnumExtension
    {
        /// <summary>
        /// Give associated String Value of an enum field who had specify it in its attributs
        /// </summary>
        /// <param name="value">Enum field to display</param>
        /// <returns>Associated string value of attribut founded</returns>
        /// <exception cref="System.NotSupportedException">Throw when current value enum haven't specified StringValue or had specified more than one in their attributs.</exception>
        public static string Display(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            if (!Enum.IsDefined(type, value)) throw new NotSupportedException(
                $"Enum value is not defined. Type:{type}. Value:{value}");

            // Get fieldinfo for this type
            var field = type.GetField(value.ToString());
            if (field == null) throw new NotSupportedException("Unable to get FieldInfo for this enum.");

            // Get the display attributes

            // Return the first if there was a match.
            if (!(field.GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] attribs) || attribs.Length == 0) throw new AttributeNotFoundException(typeof(DisplayAttribute), type);
            if (attribs.Length > 1) throw new AttributeSeveralFoundException(typeof(DisplayAttribute), type);
            return attribs[0].Value;
        }
    }
}
