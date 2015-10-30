namespace Contest.Core.Converters.BooleanConverter
{
    public class BooleanYN : Boolean
    {
        #region Properties

        public override string TrueValue { get { return "Y"; } }
        public override string FalseValue { get { return "N"; } }

        #endregion
    }
}
