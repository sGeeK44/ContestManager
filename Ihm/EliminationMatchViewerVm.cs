using Contest.Business;

namespace Contest.Ihm
{
    public class EliminationMatchViewerVm : MatchBaseVm
    {
        public EliminationMatchViewerVm(IMatch match, bool isFirstEliminationStep, bool isLastEliminationStep)
            : base(match)
        {
            ShowTop = !isLastEliminationStep;
            ShowBottom = !isFirstEliminationStep;
            LineTop = new LineVm();
            LineBottom = new LineVm();
        }

        public double Height { get; set; }

        public double Width { get; set; }

        public double MatchHeight { get; set; }

        public double MatchWidth { get; set; }

        public LineVm LineTop { get; set; }

        public LineVm LineBottom { get; set; }

        public bool ShowTop { get; set; }

        public bool ShowBottom { get; set; }

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

        public void OnSizeChanged()
        {
            var midPoint = Width / 2;
            var lineLength = (Height - MatchHeight) / 2;
            LineTop.X1 = LineTop.X2 = midPoint;
            LineTop.Y1 = 0;
            LineTop.Y2 = lineLength;
            LineBottom.X1 = LineBottom.X2 = midPoint;
            LineBottom.Y1 = Height - lineLength;
            LineBottom.Y2 = Height;
        }
    }
}
