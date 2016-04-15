using System;

namespace Contest.Core.Serialization
{
    /// <summary>
    /// Use this attribut to define a dynamic type for a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DynamicClassAttribute : PivotAttribute
    {
        /// <summary>
        /// Constructor used to init a DynamicClassAttribute Attribute
        /// </summary>
        /// <param name="enumPivot">Enum in wich fields is set with TmcValue and Associated Class to make relation</param>
        public DynamicClassAttribute(Type enumPivot) : base (enumPivot) { }
    }
}
