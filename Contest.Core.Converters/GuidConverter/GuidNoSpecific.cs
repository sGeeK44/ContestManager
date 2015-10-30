namespace Contest.Core.Converters.GuidConverter
{
    public class GuidNoSpecific : StringGuid
    {
        #region Properties

        /// <summary>
        /// Represent format for Guid ex:'yyyyMMdd'
        /// </summary>
        public override string Pattern { get { return null; } }

        #endregion

        public GuidNoSpecific() { }
    }
}
