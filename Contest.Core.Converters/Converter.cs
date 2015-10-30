using System;
using System.Linq;
using Contest.Core.Converters.AttributeConverter;
using Contest.Core.Converters.BooleanConverter;
using Contest.Core.Converters.DateTimeConverter;
using Contest.Core.Converters.EnumConverter;
using Contest.Core.Converters.GuidConverter;
using Contest.Core.Converters.NumberConverter;
using Contest.Core.Converters.TimeSpanConverter;

namespace Contest.Core.Converters
{
    /// <summary>
    /// Defines methods that convert string  value to a common language runtime type that has an equivalent value and reverse.
    /// </summary>
    public class Converter : IConverter
    {
        private static readonly Converter instance = new Converter();
        private static readonly IBooleanConverter defaultBooleanConverter = new BooleanTrueFalse();
        private static readonly INumberConverter defaultNumberConverter = new NumberNoSpecific();
        private static readonly IEnumConverter defaultEnumConverter = new StringEnumByFieldName();
        private static readonly IDateTimeConverter defaultDateTimeConverter = new DateTimeTimestamp();
        private static readonly ITimeSpanConverter defaultTimeSpanConverter = new TimeSpanDb2();
        private static readonly IGuidConverter defaultGuidConverter = new GuidNoSpecific();

        /// <summary>
        /// Get converter
        /// </summary>
        public static Converter Instance { get { return instance; } }

        protected Converter() { }

        /// <summary>
        /// Convert object into string representation
        /// </summary>
        /// <param name="obj">object to convert</param> 
        /// <param name="customAttr">Custom attribut on which specify Specific convert is specified</param>
        /// <returns>string representation</returns>
        public string Convert(object obj, object[] customAttr)
        {
            if (obj == null) return string.Empty;

            var objectType = obj.GetType();
            if (objectType == typeof (bool)) return ToString((bool) obj, customAttr);
            if (objectType == typeof(ushort)) return ToString((ushort)obj, customAttr);
            if (objectType == typeof(short)) return ToString((short)obj, customAttr);
            if (objectType == typeof(uint)) return ToString((uint)obj, customAttr);
            if (objectType == typeof(int)) return ToString((int)obj, customAttr);
            if (objectType == typeof(ulong)) return ToString((ulong)obj, customAttr);
            if (objectType == typeof(long)) return ToString((long)obj, customAttr);
            if (objectType == typeof(double)) return ToString((double)obj, customAttr);
            if (objectType == typeof(float)) return ToString((float)obj, customAttr);
            if (objectType == typeof(string)) return ToString((string)obj, customAttr);
            if (objectType.IsEnum) return ToString((Enum)obj, customAttr);
            if (objectType == typeof(TimeSpan)) return ToString((TimeSpan)obj, customAttr);
            if (objectType == typeof(DateTime)) return ToString((DateTime)obj, customAttr);
            if (objectType == typeof (Guid)) return ToString((Guid)obj, customAttr);
            return null;
        }

        /// <summary>
        /// Convert an string value into object on specified type
        /// </summary>
        /// <param name="objectType">Type of object to instance</param>
        /// <param name="value">string to convert</param>
        /// <param name="customAttr">Custom attribut on which specify Specific convert is specified</param>
        /// <returns>Object instancied</returns>
        public object Convert(Type objectType, string value, object[] customAttr)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");

            if (objectType == typeof (bool)) return ToBoolean(value, customAttr);
            if (objectType == typeof (ushort)) return ToUShort(value, customAttr);
            if (objectType == typeof (short)) return ToShort(value, customAttr);
            if (objectType == typeof (uint)) return ToUInt(value, customAttr);
            if (objectType == typeof (int)) return ToInt(value, customAttr);
            if (objectType == typeof (ulong)) return ToULong(value, customAttr);
            if (objectType == typeof (long)) return ToLong(value, customAttr);
            if (objectType == typeof (double)) return ToDouble(value, customAttr);
            if (objectType == typeof (float)) return ToFloat(value, customAttr);
            if (objectType == typeof (string)) return ToString(value, customAttr);
            if (objectType.IsEnum) return ToEnum(value, objectType, customAttr);
            if (objectType == typeof (TimeSpan)) return ToTimeSpan(value, customAttr);
            if (objectType == typeof(DateTime)) return ToDateTime(value, customAttr);
            if (objectType == typeof(Guid)) return ToGuid(value, customAttr);
            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                var args = objectType.GetGenericArguments();
                if (args.Length != 1)
                    throw new NotSupportedException(string.Format("Converter doesn't support this type of nullable. TypeO:{0}", objectType));
                try
                {
                    return Convert(args[0], value, customAttr);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private static T SeekSpecificFormat<T>(object[] customAttr) where T : ConverterAttribute
        {
            if (customAttr == null) return default(T);

            //Seek about specific format in specified custom attribute
            var formatList = customAttr.Where(attr => attr.GetType() == typeof(T))
                                                        .Cast<ConverterAttribute>()
                                                        .ToArray();
            switch (formatList.Count())
            {
                case 1:
                    return (T)formatList[0];
                case 0:
                    return default(T);
                default:
                    throw new ConverterSettingException(string.Format("Customs attributes can not specified more than one specific {0}. Founded: {1}", typeof(T).Name, formatList.Count()));
            }
        }

        protected string ToString(bool @bool, object[] customAttr)
        {
            var format = SeekSpecificFormat<BooleanAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultBooleanConverter;
            return converter.ToString(@bool);
        }

        protected string ToString(ushort @ushort, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@ushort);
        }

        protected string ToString(short @short, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@short);
        }

        protected string ToString(uint @uint, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@uint);
        }

        protected string ToString(int @int, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@int);
        }

        protected string ToString(ulong @ulong, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@ulong);
        }

        protected string ToString(long @long, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@long);
        }

        protected string ToString(double @double, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@double);
        }

        protected string ToString(float @float, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToString(@float);
        }

        protected string ToString(string @string, object[] customAttr)
        {
            return @string;
        }

        protected string ToString(Enum @enum, object[] customAttr)
        {
            var format = SeekSpecificFormat<EnumAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultEnumConverter;
            return converter.ToString(@enum);
        }

        protected string ToString(TimeSpan timeSpan, object[] customAttr)
        {
            var format = SeekSpecificFormat<TimeSpanAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultTimeSpanConverter;
            return converter.ToString(timeSpan);
        }

        protected string ToString(DateTime dateTime, object[] customAttr)
        {
            var format = SeekSpecificFormat<DateTimeAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultDateTimeConverter;
            return converter.ToString(dateTime);
        }

        protected string ToString(Guid guid, object[] customAttr)
        {
            var format = SeekSpecificFormat<GuidAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultGuidConverter;
            return converter.ToString(guid);
        }

        public bool ToBoolean(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<BooleanAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultBooleanConverter;
            return converter.ToBoolean(value);
        }

        public double ToDouble(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToDouble(value);
        }

        public short ToShort(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToShort(value);
        }

        public uint ToUInt(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToUInteger(value);
        }

        public int ToInt(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToInteger(value);
        }

        public ulong ToULong(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToULong(value);
        }

        public long ToLong(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToLong(value);
        }

        public float ToFloat(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToFloat(value);
        }

        public ushort ToUShort(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<NumberAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultNumberConverter;
            return converter.ToUShort(value);
        }

        protected Enum ToEnum(string value, Type objecType, object[] customAttr)
        {
            var format = SeekSpecificFormat<EnumAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultEnumConverter;
            return converter.ToEnum(value, objecType);
        }

        public DateTime ToDateTime(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<DateTimeAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultDateTimeConverter;
            return converter.ToDateTime(value);
        }

        public TimeSpan ToTimeSpan(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<TimeSpanAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultTimeSpanConverter;
            return converter.ToTimeSpan(value);
        }

        public Guid ToGuid(string value, object[] customAttr)
        {
            var format = SeekSpecificFormat<GuidAttribute>(customAttr);
            var converter = format != null ? format.Converter : DefaultGuidConverter;
            return converter.ToGuid(value);
        }

        protected virtual IBooleanConverter DefaultBooleanConverter
        {
            get { return defaultBooleanConverter; }
        }

        protected virtual INumberConverter DefaultNumberConverter
        {
            get { return defaultNumberConverter; }
        }

        protected virtual IEnumConverter DefaultEnumConverter
        {
            get { return defaultEnumConverter; }
        }

        protected virtual IDateTimeConverter DefaultDateTimeConverter
        {
            get { return defaultDateTimeConverter; }
        }

        protected virtual ITimeSpanConverter DefaultTimeSpanConverter
        {
            get { return defaultTimeSpanConverter; }
        }

        protected virtual IGuidConverter DefaultGuidConverter
        {
            get { return defaultGuidConverter; }
        }
    }
}