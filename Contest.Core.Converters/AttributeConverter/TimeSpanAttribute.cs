using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class TimeSpanAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default time span converter
        /// </summary>
        /// <returns>A time span converter</returns>
        public ITimeSpanConverter Converter
        {
            get { return Instances[_type] as ITimeSpanConverter; }
        }

        #endregion

        #region Constructors

        public TimeSpanAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(ITimeSpanConverter), true))
                throw new ArgumentException("Converter have to implement ITimeSpanConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
