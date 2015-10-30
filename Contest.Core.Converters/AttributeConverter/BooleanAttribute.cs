using System;

namespace Contest.Core.Converters.AttributeConverter
{
    public class BooleanAttribute : ConverterAttribute
    {
        #region Fields

        private readonly Type _type;

        #endregion

        #region Properties

        /// <summary>
        /// Get specified of default boolean converter
        /// </summary>
        /// <returns>A boolean converter</returns>
        public IBooleanConverter Converter
        {
            get { return Instances[_type] as IBooleanConverter; }
        }

        #endregion

        #region Constructors

        public BooleanAttribute(Type typeOfConverter)
            : base(typeOfConverter)
        {
            if (!typeOfConverter.IsDefined(typeof(IBooleanConverter), true))
                throw new ArgumentException("Converter have to implement IBooleanConverter interface.", "typeOfConverter");

            _type = typeOfConverter;
        }

        #endregion
    }
}
