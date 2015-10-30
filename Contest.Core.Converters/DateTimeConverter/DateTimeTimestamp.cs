namespace Contest.Core.Converters.DateTimeConverter
{
    public class DateTimeTimestamp : StringDateTime
    {
        #region Constantes

        private const string TIMESTAMP_FORMAT = "yyyy-MM-dd-HH.mm.ss.ffffff";

        #endregion

        #region Properties

        /// <summary>
        /// Represent format for DateTime ex:'yyyyMMdd'
        /// </summary>
        public override string Pattern { get { return TIMESTAMP_FORMAT; } }

        #endregion
    }
}
