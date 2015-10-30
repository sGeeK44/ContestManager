namespace Contest.Core.Converters.TimeSpanConverter
{
    public class TimeSpanSpecific : StringTimeSpan
    {
        private readonly string _pattern;

        #region Properties

        /// <summary>
        /// Represent format for TimeSpan ex:'hhmmss'
        /// </summary>
        public override string Pattern { get { return _pattern; } }

        #endregion

        public TimeSpanSpecific(string pattern)
        {
            _pattern = pattern;
        }
    }
}
