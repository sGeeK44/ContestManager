using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class NumberAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default number converter
        /// </summary>
        /// <returns>A number converter</returns>
        public INumberConverter Converter
        {
            get { return Instances[_type] as INumberConverter; }
        }

        #endregion

        #region Constructors

        public NumberAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(INumberConverter), true))
                throw new ArgumentException("Converter have to implement INumberConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
