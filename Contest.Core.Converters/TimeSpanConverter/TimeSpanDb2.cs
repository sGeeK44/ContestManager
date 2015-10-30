namespace Contest.Core.Converters.TimeSpanConverter
{
    public class TimeSpanDb2 : StringTimeSpan
    {
        #region Constantes

        /// <summary>
        /// Represent a numeric format for timespan 'hhmmss'
        /// </summary>
        private const string TIME_DB2_FORMAT = @"hh\.mm\.ss";

        #endregion

        #region Properties

        public override string Pattern { get { return TIME_DB2_FORMAT; } }

        #endregion
    }
}
