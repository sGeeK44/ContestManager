using System;
using System.Globalization;
using System.Windows.Data;
using Contest.Core.Windows;

namespace Contest.Core.Converters
{
    public class DisplayEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                if (!targetType.IsEnum) throw new ArgumentException(string.Format("This converter is only desgin for Enum. Type:{0}", targetType), "targetType");
                return DisplayAttribute.Parse(targetType, string.Empty, false);
            }
            var enumValue = value as Enum;
            return enumValue == null ? string.Empty : enumValue.Display();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException("value");
            return DisplayAttribute.Parse(targetType, value as string, false);
        }
    }
}
