using System;

namespace Contest.Core.Serialization
{
    /// <summary>
    /// Use this attribut to define a generic type for a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenericClassAttribute : PivotAttribute
    {
        /// <summary>
        /// Constructor used to init a GenericClassAttribute Attribute
        /// </summary>
        /// <param name="enumPivot">Enum in wich fields is set with value and Associated Class to make relation</param>
        public GenericClassAttribute(Type enumPivot) : base(enumPivot) { }
    }
}
