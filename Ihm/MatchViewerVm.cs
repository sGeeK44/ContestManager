using Contest.Domain.Matchs;

namespace Contest.Ihm
{
    public class MatchViewerVm : MatchBaseVm
    {
        public MatchViewerVm(IMatch match)
            : base(match)
        {
        }

        public override bool ShowScoreBlock => IsEnded;

        public override bool ShowUpdateScoreBox => false;

        public override bool ShowVs => !IsEnded;
    }
}
