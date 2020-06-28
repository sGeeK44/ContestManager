using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class LineVm : ViewModel
    {
        private double _x1;
        private double _x2;
        private double _y1;
        private double _y2;

        public LineVm() {}

        public LineVm(double x1, double x2, double y1, double y2)
        {
            _x1 = x1;
            _x2 = x2;
            _y1 = y1;
            _y2 = y2;
        }

        public double X1
        {
            get => _x1;
            set => Set(ref _x1, value);
        }

        public double X2
        {
            get => _x2;
            set => Set(ref _x2, value);
        }

        public double Y1
        {
            get => _y1;
            set => Set(ref _y1, value);
        }

        public double Y2
        {
            get => _y2;
            set => Set(ref _y2, value);
        }
    }
}
