namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanWildCardBlank : Boolean
    {
        #region Properties

        public override string TrueValue { get { return "*"; } }
        public override string FalseValue { get { return string.Empty; } }

        #endregion
    }
}
