using System;
using System.Globalization;
using System.Windows.Data;

namespace Contest.Core.Converters
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is bool) return !(bool)value;
            throw new InvalidOperationException(string.Format("Invalid input value of type '{0}'", value.GetType()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
