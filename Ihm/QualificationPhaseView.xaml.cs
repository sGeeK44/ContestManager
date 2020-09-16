using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using Contest.Domain.Games;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for Contest.xaml
    /// </summary>
    public partial class QualificationPhaseView
    {
        private double _currentOffset = 0;
        private bool _goDown = true;
        private DispatcherTimer _timer;

        public QualificationPhaseView(IList<IGameStep> gameStepList)
        {
            if (gameStepList == null) throw new ArgumentNullException(nameof(gameStepList));

            InitializeComponent();
            DataContext = new QualificationPhaseViewVm(gameStepList);
            _timer = new DispatcherTimer();
            _timer.Tick += Scroll;
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Start();
        }

        private void Scroll(object sender, EventArgs e)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            if (ScrollViewer.VerticalOffset <= 0.0)
            {
                _goDown = true;
                _timer.Interval = TimeSpan.FromMilliseconds(10000);
            }
            if (ScrollViewer.VerticalOffset >= ScrollViewer.ScrollableHeight)
            {
                _goDown = false;
                _timer.Interval = TimeSpan.FromMilliseconds(10000);
            }
            if (_goDown) _currentOffset += 0.1;
            else _currentOffset -= 0.1;
            ScrollViewer.ScrollToVerticalOffset(_currentOffset);
        }
    }
}
