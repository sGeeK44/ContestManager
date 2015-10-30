namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanON : Boolean
    {
        #region Properties

        public override string TrueValue { get { return "O"; } }
        public override string FalseValue { get { return "N"; } }

        #endregion
    }
}
