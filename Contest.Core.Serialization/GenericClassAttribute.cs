using System;
using Contest.Core.Converters;
using Contest.Core.Exceptions;

namespace Contest.Core.Serialization
{
    /// <summary>
    /// Use this attribut to define a generic type for a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenericClassAttribute : Attribute
    {
        internal Type EnumType { get; private set; }

        /// <summary>
        /// Constructor used to init a GenericClassAttribute Attribute
        /// </summary>
        /// <param name="enumPivot">Enum in wich fields is set with value and Associated Class to make relation</param>
        public GenericClassAttribute(Type enumPivot)
        {
            if (enumPivot == null) throw new ArgumentNullException("enumPivot");
            if (!enumPivot.IsEnum) throw new InvalidTypeException(typeof(Enum), enumPivot);

            EnumType = enumPivot;
        }
        
        /// <summary>
        /// Determine dynamic type
        /// </summary>
        /// <param name="value">String value of enum pivot</param>
        /// <param name="converter">Converter to convert string value to enum pivot field</param>
        /// <returns></returns>
        internal Type GetGenericType(string value, IEnumConverter converter)
        {
            if (converter == null) throw new ArgumentNullException("converter");

            // Get enum field from string value
            var @enum = converter.ToEnum(value, EnumType);

            //Return associated class specified on enum field.
            return @enum.GetAssociatedClass();
        }
    }
}
