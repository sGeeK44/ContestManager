using System;
using System.Linq;
using Contest.Core.Converters;
using Contest.Core.Exceptions;

namespace Contest.Core.Serialization
{
    public static class GenericClassAttributeExtension
    {
        /// <summary>
        /// Indicate if current object type is generic or not
        /// </summary>
        /// <param name="objType">Object type to test</param>
        /// <param name="typeOfPivot">Type of enum pivot if object is dynamic</param>
        /// <returns>True if current object is generic, false else</returns>
        public static bool IsGenericType(this Type objType, out Type typeOfPivot)
        {
            if (objType == null) throw new NullReferenceException();
            var attr = objType.GetCustomAttributes(typeof(GenericClassAttribute), false).FirstOrDefault() as GenericClassAttribute;
            if (attr == null)
            {
                typeOfPivot = null;
                return false;
            }
            typeOfPivot = attr.EnumType;
            return true;
        }
        
        /// <summary>
        /// Determine generic type of current object
        /// </summary>
        /// <param name="objType">Object type to get generic type</param>
        /// <param name="value">String value of enum pivot</param>
        /// <param name="converter">Converter to convert string value to enum pivot field</param>
        /// <returns>Real type</returns>
        public static Type GetGenericType(this Type objType, string value, IEnumConverter converter)
        {
            var attr = objType.GetCustomAttributes(typeof(GenericClassAttribute), false) as GenericClassAttribute[];

            if (attr == null || attr.Length == 0) return objType;
            if (attr.Length == 1) return attr[0].GetGenericType(value, converter);
            throw new SeveralFoundException(typeof(GenericClassAttribute), attr.Length);
        }
    }
}