namespace Contest.Core.Converters.TimeSpanConverter
{
    public class TimeSpanHost : StringTimeSpan
    {
        #region Constantes

        /// <summary>
        /// Represent a numeric format for timespan 'hhmmss'
        /// </summary>
        private const string NUMERIC_TIME_FORMAT = "hhmmss";

        #endregion

        #region Properties

        public override string Pattern { get { return NUMERIC_TIME_FORMAT; } }

        #endregion
    }
}
