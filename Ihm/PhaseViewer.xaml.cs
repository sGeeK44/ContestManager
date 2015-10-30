using System;
using System.Windows.Threading;
using Contest.Business;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for MatchList.xaml
    /// </summary>
    public partial class PhaseViewer
    {
        private double _currentOffset;
        private bool _goDown = true;

        public PhaseViewer(IContest contest)
        {
            InitializeComponent();
            DataContext = new PhaseViewerVm(contest);
            var timer = new DispatcherTimer();
            timer.Tick += Scroll;
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Start();
        }

        private void Scroll(object sender, EventArgs e)
        {
            if (ScrollViewer.VerticalOffset <= 0.0) _goDown = true;
            if (ScrollViewer.VerticalOffset >= ScrollViewer.ScrollableHeight) _goDown = false;
            if (_goDown) _currentOffset += 0.1;
            else _currentOffset -= 0.1;
            ScrollViewer.ScrollToVerticalOffset(_currentOffset);
        }
    }
}
