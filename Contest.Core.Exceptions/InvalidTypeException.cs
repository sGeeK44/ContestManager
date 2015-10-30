using System;
using System.Text;

namespace Contest.Core.Exceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(Type excepted, Type actual)
            : base(FormatMessage(new[] { excepted }, actual))
        { }

        public InvalidTypeException(Type[] excepted, Type actual)
            : base(FormatMessage(excepted, actual))
        { }

        private static string FormatMessage(Type[] excepted, Type actual)
        {
            if (excepted == null) throw new ArgumentNullException("excepted");
            if (actual == null) throw new ArgumentNullException("actual");
            var result = new StringBuilder("Invalid object type. expected:");
            foreach (var item in excepted)
            {
                result.AppendFormat(" {0}", item.Name);
            }
            result.AppendFormat(", actual: {0}", actual.Name);
            return result.ToString();
        }
    }
}
