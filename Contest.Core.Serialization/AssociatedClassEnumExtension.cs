using System;
using Contest.Core.Exceptions;

namespace Contest.Core.Serialization
{
    public static class AssociatedClassEnumExtension
    {
        /// <summary>
        /// Give Associated Class Type of an enum who had specify it in their attributs
        /// </summary>
        /// <returns>Value of first AssociatedClass attribut founded, else null if no one found</returns>
        public static Type GetAssociatedClass(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(AssociatedClassAttribute), false) as AssociatedClassAttribute[];

            // Return the first if there was a match.
            if (attribs == null || attribs.Length == 0) throw new NotFoundException(typeof(AssociatedClassAttribute));
            if (attribs.Length > 1) throw new SeveralFoundException(typeof(AssociatedClassAttribute), attribs.Length);
            return attribs[0].Class;
        }
    }
}