using Contest.Business;

namespace Contest.Ihm
{
    public class MatchViewerVm : MatchBaseVm
    {
        public MatchViewerVm(IMatch match)
            : base(match)
        {
        }

        public override bool ShowScoreBlock
        {
            get { return IsEnded; }
        }

        public override bool ShowUpdateScoreBox
        {
            get { return false; }
        }

        public override bool ShowVs
        {
            get { return !IsEnded; }
        }
    }
}
