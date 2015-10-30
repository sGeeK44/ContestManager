using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class PhaseViewerVm : ViewModel
    {
        private string _currentTime;
        private int _index;
        private PhaseViewItem _current;

        public PhaseViewerVm(IContest contest)
        {
            if (contest == null) throw new ArgumentNullException("contest");

            PhaseList = contest.PhaseList.Select(_ => new PhaseViewItem(_)).ToList();
            contest.NewPhaseLaunch += (sender, phase) => PhaseList.Add(new PhaseViewItem(phase));
            var timer = new DispatcherTimer();
            timer.Tick += RefreshClock;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            
            WatchNextPhase();

            timer = new DispatcherTimer();
            timer.Tick += ChangePhase;
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Start();
        }

        private void ChangePhase(object sender, EventArgs e)
        {
            WatchNextPhase();
        }

        private void WatchNextPhase()
        {
            while (true)
            {
                if (_index >= PhaseList.Count) _index = 0;
                if (PhaseList.Count == 0) return;
                var next = PhaseList[_index];
                if (next.IsFinished)
                {
                    PhaseList.Remove(next);
                    continue;
                }
                Current = PhaseList[_index];
                _index++;
                break;
            }
        }

        public string CurrentTime
        {
            get { return _currentTime; }
            set { Set(ref _currentTime, value); }
        }

        private void RefreshClock(Object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("H:mm:ss");
        }

        public PhaseViewItem Current
        {
            get { return _current; }
            set { Set(ref _current, value); }
        }

        public List<PhaseViewItem> PhaseList { get; set; }

        public class PhaseViewItem : ViewModel
        {
            private IPhase _phase;
            private object _instance;
            private const int MAX_NEXT_GAME_DISPLAY = 5;
            private const int MAX_FINISHED_GAME_DISPLAY = 5;

            public PhaseViewItem(IPhase phase)
            {
                if (phase == null) throw new ArgumentNullException("phase");
                _phase = phase;
                switch (_phase.Type)
                {
                    case PhaseType.Qualification:
                        Title = "Qualification";
                        Instance = new QualificationPhaseView(phase.GameStepList);
                        break;
                    case PhaseType.Main:
                        Title = "Principale";
                        Instance = new EliminationPhaseView(phase);
                        break;
                    case PhaseType.Consoling:
                        Title = "Consolante";
                        Instance = new EliminationPhaseView(phase);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Ce type de phase n'est pas géré. Type:{0}.", phase.Type));
                }

                NextGameList = new ObservableCollection<MatchViewerVm>();
                NextGameListWaiting = new ObservableCollection<MatchViewerVm>();
                InProgressGameList = new ObservableCollection<MatchViewerVm>();
                FinishedGameList = new ObservableCollection<MatchViewerVm>();

                foreach (var match in ManagePhaseVm.SortedMatch(_phase))
                {
                    CreateMatchVM(match);
                }

                _phase.NextStepStarted += (sender, gameStep) =>
                {
                    foreach (var match in ManagePhaseVm.SortedMatch(_phase))
                    {
                        CreateMatchVM(match);
                    }
                };
            }

            public string Title { get; set; }

            public ObservableCollection<MatchViewerVm> NextGameListWaiting { get; set; }

            public ObservableCollection<MatchViewerVm> NextGameList { get; set; }

            public ObservableCollection<MatchViewerVm> InProgressGameList { get; set; }

            public ObservableCollection<MatchViewerVm> FinishedGameList { get; set; }

            public bool HasMatchEnded
            {
                get { return FinishedGameList.Count != 0; }
            }

            public bool HasMatchInProgress
            {
                get { return InProgressGameList.Count != 0; }
            }

            public bool HasMatchPlanned
            {
                get { return NextGameList.Count != 0; }
            }

            public bool IsFinished { get { return _phase.IsFinished; } }

            public object Instance
            {
                get { return _instance; }
                set { Set(ref _instance, value); }
            }

            private void CreateMatchVM(IMatch match)
            {
                var matchVM = new MatchViewerVm(match);
                if (!match.IsBeginning && NextGameList.Count < MAX_NEXT_GAME_DISPLAY) NextGameList.Add(matchVM);
                else if (!match.IsBeginning) NextGameListWaiting.Add(matchVM);
                else if (!match.IsFinished) InProgressGameList.Add(matchVM);
                else if (match.IsFinished && FinishedGameList.Count < MAX_NEXT_GAME_DISPLAY) FinishedGameList.Add(matchVM);

                match.MatchEnded += sender =>
                {
                    InProgressGameList.Remove(matchVM);
                    if (FinishedGameList.Count >= MAX_FINISHED_GAME_DISPLAY) FinishedGameList.RemoveAt(0);
                    FinishedGameList.Add(matchVM);
                    OnPropertyChanged(() => HasMatchEnded);
                    OnPropertyChanged(() => HasMatchInProgress);
                };
                match.MatchStarted += sender =>
                {
                    NextGameList.Remove(matchVM);
                    if (NextGameListWaiting.Count > 0)
                    {
                        var next = NextGameListWaiting.First();
                        NextGameList.Add(next);
                        NextGameListWaiting.Remove(next);
                    }
                    InProgressGameList.Add(matchVM);
                    OnPropertyChanged(() => HasMatchPlanned);
                    OnPropertyChanged(() => HasMatchInProgress);
                };
            }
        }
    }
}
