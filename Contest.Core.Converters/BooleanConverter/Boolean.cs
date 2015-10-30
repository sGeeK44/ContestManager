namespace Contest.Core.Converters.BooleanConverter
{
    public abstract class Boolean : IBooleanConverter
    {
        public abstract string TrueValue { get; }
        public abstract string FalseValue { get; }

        #region Constructors

        protected Boolean()
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Convert boolean in .NET representation to equivalent boolean object representation in target system
        /// </summary>
        /// <param name="value">A boolean in .NET</param>
        /// <returns>A boolean object in String system</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public string ToString(bool value)
        {
            return value ? TrueValue : FalseValue;
        }

        /// <summary>
        /// Convert boolean object for String system to equivalent .NET representation
        /// </summary>
        /// <param name="value">A boolean object in String system</param>
        /// <returns>A boolean  in .NET</returns>
        /// <exception cref="ConverterException"/>
        /// <exception cref="ConverterSettingException"/>
        public bool ToBoolean(string value)
        {
            if (string.Equals(TrueValue, value)) return true;
            if (string.Equals(FalseValue, value)) return false;
            throw new ConverterException(string.Format("Specified string doesn't corresponds to a valid value for a boolean. Value:{0}", value));
        }

        #endregion
    }
}
