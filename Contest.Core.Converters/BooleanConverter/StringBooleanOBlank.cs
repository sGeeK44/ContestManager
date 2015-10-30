namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanOBlank : Boolean
    {
        #region Properties

        public override string TrueValue { get { return "O"; } }
        public override string FalseValue { get { return string.Empty; } }

        #endregion
    }
}
