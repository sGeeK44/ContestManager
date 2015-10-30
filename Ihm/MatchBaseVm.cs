using System;
using System.Windows.Threading;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public abstract class MatchBaseVm : ViewModel
    {
        public IMatch CurrentMatch { get; private set; }
        private ushort? _teamScore1;
        private ushort? _teamScore2;
        private string _playGroundNumber;
        private bool _isStarted;
        private bool _isEnded;
        private bool _isInProgess;
        private string _elapseTime;

        public MatchBaseVm(IMatch match)
        {
            if (match == null) throw new ArgumentException();
            
            var timer = new DispatcherTimer();
            timer.Tick += RefreshTimeElapse;
            timer.Interval = TimeSpan.FromSeconds(1);
            CurrentMatch = match;
            UpdateScore();
            UpdateState();
            UpdateField();
            RefreshTimeElapse(null, null);

            CurrentMatch.MatchStarted += sender =>
            {
                timer.Start();
                UpdateField();
                UpdateState(); 
            };
            CurrentMatch.MatchEnded += sender =>
            {
                timer.Stop();
                UpdateScore();
                UpdateState();
            };
            CurrentMatch.ScoreChanged += sender => UpdateScore();
        }

        private void RefreshTimeElapse(object sender, EventArgs e)
        {
            var elapse = CurrentMatch.Elapse;
            ElapseTime = elapse != null ? string.Format("{0:mm}:{0:ss}", elapse) : string.Empty;
        }

        protected void UpdateScore()
        {
            if (!CurrentMatch.IsBeginning) return;
            TeamScore1 = CurrentMatch.TeamScore1;
            TeamScore2 = CurrentMatch.TeamScore2;
        }

        protected void UpdateField()
        {
            PlayGroundNumber = CurrentMatch.MatchField != null ? CurrentMatch.MatchField.Name : string.Empty;
        }

        protected void UpdateState()
        {
            switch (CurrentMatch.MatchState)
            {
                case MatchState.Planned:
                    IsStarted = false;
                    IsInProgress = false;
                    IsEnded = false;
                    break;
                case MatchState.InProgress:
                    IsStarted = true;
                    IsInProgress = true;
                    IsEnded = false;
                    break;
                case MatchState.Finished:
                    IsStarted = true;
                    IsInProgress = false;
                    IsEnded = true;
                    break;
            }
        }

        public bool IsStarted
        {
            get { return _isStarted; }
            set
            {
                Set(ref _isStarted, value);
                OnPropertyChanged(() => ShowVs);
                OnPropertyChanged(() => ShowScoreBlock);
                OnPropertyChanged(() => ShowUpdateScoreBox);
            }
        }

        public bool IsInProgress
        {
            get { return _isInProgess; }
            set
            {
                Set(ref _isInProgess, value);
                OnPropertyChanged(() => ShowVs);
                OnPropertyChanged(() => ShowScoreBlock);
                OnPropertyChanged(() => ShowUpdateScoreBox);
            }
        }

        public bool IsEnded
        {
            get { return _isEnded; }
            set
            {
                Set(ref _isEnded, value);
                OnPropertyChanged(() => ShowVs);
                OnPropertyChanged(() => ShowScoreBlock);
                OnPropertyChanged(() => ShowUpdateScoreBox);
            }
        }

        public string TeamName1
        {
            get { return CurrentMatch.Team1.Name; }
        }

        public ushort? TeamScore1
        {
            get { return _teamScore1; }
            set { Set(ref _teamScore1, value); }
        }

        public string TeamName2
        {
            get { return CurrentMatch.Team2.Name; }
        }

        public ushort? TeamScore2
        {
            get { return _teamScore2; }
            set { Set(ref _teamScore2, value); }
        }

        public string PlayGroundNumber
        {
            get { return _playGroundNumber; }
            set { Set(ref _playGroundNumber, value); }
        }

        public string ElapseTime
        {
            get { return _elapseTime; }
            set { Set(ref _elapseTime, value); }
        }

        public abstract bool ShowScoreBlock { get; }

        public abstract bool ShowUpdateScoreBox { get; }

        public abstract bool ShowVs { get; }
    }
}
