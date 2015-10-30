using System;

namespace Contest.Core.Exceptions
{
    /// <summary>
    /// Represent an error when a unik search into database was done and no result was found
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of Cdm.Spb1.Core.Exceptions.NotFoundException
        /// </summary>
        /// <param name="type">Type of object not found</param>
        public NotFoundException(Type type)
            : base(string.Format("None {0} corresponds.", type.Name))
        { }
    }
}
