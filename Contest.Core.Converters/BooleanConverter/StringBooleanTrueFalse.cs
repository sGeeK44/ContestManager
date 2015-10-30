namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanTrueFalse : Boolean
    {
        #region Properties

        public override string TrueValue { get { return true.ToString(); } }
        public override string FalseValue { get { return false.ToString(); } }

        #endregion
    }
}
