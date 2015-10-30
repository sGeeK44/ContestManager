using System;

namespace Contest.Core.Serialization
{
    /// <summary>
    /// This attribute is used to associate a Class to an enum field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AssociatedClassAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Holds the Type for a value in an enum.
        /// </summary>
        public Type Class { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a AssociatedClass Attribute
        /// </summary>
        /// <param name="value">Fully qualified path of associated class</param>
        public AssociatedClassAttribute(Type value)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (!value.IsClass) throw new ArgumentException("Value does not represent Class", "value");
            Class = value;
        }

        #endregion
    }
}
