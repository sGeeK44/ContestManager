using System;
using System.Text;

namespace Contest.Core.Converters
{
    public class ConverterSettingException : ConverterException
    {
        public ConverterSettingException(string message)
            : base(message) { }

        public ConverterSettingException(string message, Exception innerException)
            : base(message, innerException) { }

        public ConverterSettingException(Type excepted, Type actual)
            : base(FormatMessage(new[] { excepted }, actual))
        { }

        public ConverterSettingException(Type[] excepted, Type actual)
            : base(FormatMessage(excepted, actual))
        { }

        public ConverterSettingException(Type excepted, Type actual, Exception innerException)
            : base(FormatMessage(new[] { excepted }, actual), innerException)
        { }

        public ConverterSettingException(Type[] excepted, Type actual, Exception innerException)
            : base(FormatMessage(excepted, actual), innerException)
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
