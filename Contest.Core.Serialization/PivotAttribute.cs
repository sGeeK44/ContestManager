using System;
using Contest.Core.Converters;
using Contest.Core.Exceptions;

namespace Contest.Core.Serialization
{
    public class PivotAttribute : Attribute
    {
        public Type EnumType { get; set; }

        /// <summary>
        /// Constructor used to init a DynamicClassAttribute Attribute
        /// </summary>
        /// <param name="enumPivot">Enum in wich fields is set with TmcValue and Associated Class to make relation</param>
        public PivotAttribute(Type enumPivot)
        {
            if (enumPivot == null) throw new ArgumentNullException("enumPivot");
            if (!enumPivot.IsEnum) throw new InvalidTypeException(typeof(Enum), enumPivot);

            EnumType = enumPivot;
        }

        /// <summary>
        /// Determine associated class for string value pivot
        /// </summary>
        /// <param name="value">String value of enum pivot</param>
        /// <param name="converter">Converter to convert string value to enum pivot field</param>
        /// <returns></returns>
        public Type GetAssociatedClass(string value, IConverter converter)
        {
            if (converter == null) throw new ArgumentNullException("converter");

            // Get enum field from string value
            var @enum = (Enum)converter.Convert(EnumType, value, null);

            //Return associated class specified on enum field.
            return @enum.GetAssociatedClass();
        }
    }
}