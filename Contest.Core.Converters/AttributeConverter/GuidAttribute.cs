using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class GuidAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default date time converter
        /// </summary>
        /// <returns>A date time converter</returns>
        public IGuidConverter Converter
        {
            get { return Instances[_type] as IGuidConverter; }
        }

        #endregion

        #region Constructors

        public GuidAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(IGuidConverter), true))
                throw new ArgumentException("Converter have to implement IGuidConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
