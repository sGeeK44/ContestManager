namespace Contest.Core.Converters.TimeSpanConverter
{
    public class TimeSpanHostWithoutSec : StringTimeSpan
    {
        #region Constantes

        /// <summary>
        /// Represent a numeric format for timespan 'hhmmss'
        /// </summary>
        private const string TIME_HOST_HH_MM = "hhmm";

        #endregion

        #region Properties

        public override string Pattern { get { return TIME_HOST_HH_MM; } }

        #endregion
    }
}
