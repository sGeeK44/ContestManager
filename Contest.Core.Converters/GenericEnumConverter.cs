using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Contest.Core.Converters
{
    public class GenericEnumConverter : IValueConverter
    {
        private List<object> _items;

        public List<object> Items
        {
            get { return _items ?? (_items = new List<object>()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value is bool)
                return Items.ElementAtOrDefault(System.Convert.ToByte(value));
            if (value is byte)
                return Items.ElementAtOrDefault(System.Convert.ToByte(value));
            if (value is short)
                return Items.ElementAtOrDefault(System.Convert.ToInt16(value));
            if (value is int)
                return Items.ElementAtOrDefault(System.Convert.ToInt32(value));
            if (value is long)
                return Items.ElementAtOrDefault(System.Convert.ToInt32(value));
            if (value is Enum)
                return Items.ElementAtOrDefault(System.Convert.ToInt32(value));
            throw
                new InvalidOperationException(string.Format("Invalid input value of type '{0}'", value.GetType()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return Items.Where(b => b.Equals(value)).Select((a,b) => b).FirstOrDefault();
        }
    }
}
