using System;

namespace Contest.Core.Exceptions
{
    /// <summary>
    /// Represent an error when tried to use an invalid format for a string
    /// </summary>
    public class InvalidFormatException : Exception
    {
        /// <summary>
        /// Initialize a new instance of Cdm.Spb1.Core.Exceptions.InvalidFormatException with a specified message 
        /// </summary>
        /// <param name="message">The message that describe error</param>
        public InvalidFormatException(string message)
            : base(message)
        { }
    }
}
