using System;
using System.Collections.Generic;
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
        public QualificationPhaseView(IList<IGameStep> gameStepList)
        {
            if (gameStepList == null) throw new ArgumentNullException(nameof(gameStepList));

            InitializeComponent();
            DataContext = new QualificationPhaseViewVm(gameStepList);
            var timer = new DispatcherTimer();
            timer.Tick += Scroll;
            timer.Interval = TimeSpan.FromMilliseconds(1);
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
