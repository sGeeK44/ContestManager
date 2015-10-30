using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class DateTimeAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default date time converter
        /// </summary>
        /// <returns>A date time converter</returns>
        public IDateTimeConverter Converter
        {
            get { return Instances[_type] as IDateTimeConverter; }
        }

        #endregion

        #region Constructors

        public DateTimeAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(IDateTimeConverter), true))
                throw new ArgumentException("Converter have to implement IDateTimeConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
