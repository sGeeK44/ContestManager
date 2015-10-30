using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class EnumAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default enum converter
        /// </summary>
        /// <returns>A enum converter</returns>
        public IEnumConverter Converter
        {
            get { return Instances[_type] as IEnumConverter; }
        }

        #endregion

        #region Constructors

        public EnumAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(IEnumConverter), true))
                throw new ArgumentException("Converter have to implement IEnumConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
