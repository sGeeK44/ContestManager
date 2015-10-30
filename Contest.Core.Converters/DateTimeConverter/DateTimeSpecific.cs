namespace Contest.Core.Converters.DateTimeConverter
{
    public class DateTimeSpecific : StringDateTime
    {
        private readonly string _pattern;

        #region Properties

        /// <summary>
        /// Represent format for DateTime ex:'yyyyMMdd'
        /// </summary>
        public override string Pattern { get { return _pattern; } }

        #endregion

        public DateTimeSpecific(string pattern)
        {
            _pattern = pattern;
        }
    }
}
