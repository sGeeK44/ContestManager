using System.Windows;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class EliminationMatchConteneurVm : ViewModel
    {
        private double _x1;
        private double _x2;

        public EliminationMatchConteneurVm(IMatch match, bool isFirstEliminationStep, bool isLastEliminationStep)
        {
            ShowTop = !isLastEliminationStep;
            ShowBottom = !isFirstEliminationStep;
            MatchViewer = new EliminationMatchViewerVm(match, isFirstEliminationStep, isLastEliminationStep);
        }

        public EliminationMatchViewerVm MatchViewer { get; set; }

        public bool ShowTop { get; set; }

        public bool ShowBottom { get; set; }

        public double X1
        {
            get { return _x1; }
            set { Set(ref _x1, value); }
        }

        public double X2
        {
            get { return _x2; }
            set { Set(ref _x2, value); }
        }

        internal void OnSizeChanged(SizeChangedEventArgs e)
        {
            var x = e.NewSize.Width/4;
            X1 = x;
            X2 = e.NewSize.Width - x;
        }
    }
}
