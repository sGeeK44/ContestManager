namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanOneZero : Boolean
    {
        #region Properties

        public override string TrueValue { get { return "1"; } }
        public override string FalseValue { get { return "0"; } }

        #endregion
    }
}
