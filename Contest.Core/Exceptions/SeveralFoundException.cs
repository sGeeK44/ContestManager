using System;

namespace Contest.Core.Exceptions
{
    /// <summary>
    /// Represent an error when a unik search into database was done and several result was found
    /// </summary>
    public class SeveralFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of Cdm.Spb1.Core.Exceptions.SeveralFoundException
        /// </summary>
        /// <param name="type">Type of objects founded</param>
        /// <param name="numberFounded">Number of object founded</param>
        public SeveralFoundException(Type type, int numberFounded)
            : base($"Several {type.Name} corresponds. Number founded: {numberFounded}")
        { }
    }
}
