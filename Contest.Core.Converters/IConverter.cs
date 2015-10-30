using System;

namespace Contest.Core.Converters
{
    /// <summary>
    /// Defines methods that convert string value to a common language runtime type that has an equivalent value and reverse.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Convert object into string representation
        /// </summary>
        /// <param name="obj">object to convert</param> 
        /// <param name="customAttr">Custom attribut on which specify Specific convert is specified</param>
        /// <returns>string representation</returns>
        string Convert(object obj, object[] customAttr);

        /// <summary>
        /// Convert an string value into object on specified type
        /// </summary>
        /// <param name="objectType">Type of object to instance</param>
        /// <param name="value">string to convert</param>
        /// <param name="customAttr">Custom attribut on which specify Specific convert is specified</param>
        /// <returns>Object instancied</returns>
        object Convert(Type objectType, string value, object[] customAttr);
    }
}
