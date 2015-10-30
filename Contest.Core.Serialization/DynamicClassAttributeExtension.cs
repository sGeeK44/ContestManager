using System;
using System.Linq;
using Contest.Core.Converters;
using Contest.Core.Exceptions;

namespace Contest.Core.Serialization
{
    public static class DynamicClassAttributeExtension
    {
        /// <summary>
        /// Indicate if current object type is dynamic or not
        /// </summary>
        /// <param name="objType">Object type to test</param>
        /// <param name="typeOfPivot">Type of enum pivot if object is dynamic</param>
        /// <returns>True if current object is dynamic, false else</returns>
        public static bool IsDynamicType(this Type objType, out Type typeOfPivot)
        {
            if (objType == null) throw new NullReferenceException();
            //Don't look into inherited class, else we can have infinity recurvice case.
            var attr = objType.GetCustomAttributes(typeof (DynamicClassAttribute), false).FirstOrDefault() as DynamicClassAttribute;
            if (attr == null)
            {
                typeOfPivot = null;
                return false;
            }
            typeOfPivot = attr.EnumType;
            return true;
        }

        /// <summary>
        /// Determine dynamique type of current object
        /// </summary>
        /// <param name="objType">Object type to get dynamic type</param>
        /// <param name="value">String value of enum pivot</param>
        /// <param name="converter">Converter to convert string value to enum pivot field</param>
        /// <returns>Real type</returns>
        public static Type GetDynamicType(this Type objType, string value, IEnumConverter converter)
        {
            var attr = objType.GetCustomAttributes(typeof(DynamicClassAttribute), false) as DynamicClassAttribute[];

            if (attr == null || attr.Length == 0) return objType;
            if (attr.Length == 1) return attr[0].GetDynamicType(value, converter);
            throw new SeveralFoundException(typeof(DynamicClassAttribute), attr.Length);
        }
    }
}