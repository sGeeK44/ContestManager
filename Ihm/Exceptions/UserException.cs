using System;

namespace Contest.Ihm.Exceptions
{
    public class UserException : Exception
    {
        public UserException(string message)
            : base(message)
        {
        }
    }
}
