using System;

namespace Contest.Core.Converters.NumberConverter
{
    public class NumberSpecific : Number
    {
        #region Fields

        private readonly string _pattern;
        
        #endregion
        
        #region Properties

        public override string Pattern { get { return _pattern; } }

        #endregion

        #region Constructors

        public NumberSpecific(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException("pattern");
            _pattern = pattern;
        }

        #endregion
    }
}