namespace Contest.Core.Converters.DateTimeConverter
{
    public class DateTimeHost : StringDateTime
    {
        #region Constantes

        private const string NUMERIC_DATE_FORMAT = "yyyyMMdd";

        #endregion

        #region Properties

        /// <summary>
        /// Represent format for DateTime ex:'yyyyMMdd'
        /// </summary>
        public override string Pattern { get { return NUMERIC_DATE_FORMAT; } }

        #endregion
    }
}
