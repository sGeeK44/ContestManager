namespace Contest.Core.Converters.DateTimeConverter
{
    public class DateTimeDb2 : StringDateTime
    {
        #region Constantes

        private const string TEXT_DATE_FORMAT = "yyyy-MM-dd";

        #endregion

        #region Properties

        /// <summary>
        /// Represent format for DateTime ex:'yyyyMMdd'
        /// </summary>
        public override string Pattern { get { return TEXT_DATE_FORMAT; } }

        #endregion
    }
}
