using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class QualificationStepViewVm : ViewModel
    {
        private IQualificationStep _step;

        public QualificationStepViewVm(IQualificationStep step)
        {
            GroupName = step.Name;
            _step = step;
            _step.RankChanged += sender => OnPropertyChanged(() => RowList);
        }

        public string GroupName { get; set; }

        public List<RowVM> RowList
        {
            get
            {
                return _step.Rank.Select(team => new RowVM(team, _step.MatchList.Where(item => item.IsTeamInvolved(team)).ToList()))
                                 .ToList();
            }
        }

        public class RowVM : ViewModel
        {
            private int _countMatchPlayed;
            private int _countMatchWin;
            private int _countMatchLoose;
            private int _sumMarkedPoint;
            private int _sumTakePoint;
            private int _difPointTakePoint;

            public RowVM(ITeam team, IList<IMatch> matchPlayed)
            {
                if (team == null) throw new ArgumentNullException("team");
                if (matchPlayed == null) throw new ArgumentNullException("matchPlayed");

                CurrenTeam = team;
                Name = team.Name;
                MatchList = new ObservableCollection<IMatch>(matchPlayed);
                foreach (var match in matchPlayed)
                {
                    match.MatchEnded += RefreshVM;
                    match.ScoreChanged += RefreshVM;
                }

                RefreshVM(null);
            }

            public ITeam CurrenTeam { get; set; }
            public ObservableCollection<IMatch> MatchList { get; set; }

            public string Name { get; set; }

            public int CountMatchPlayed
            {
                get { return _countMatchPlayed; }
                set { Set(ref _countMatchPlayed, value); }
            }

            public int CountMatchWin
            {
                get { return _countMatchWin; }
                set { Set(ref _countMatchWin, value); }
            }

            public int CountMatchLoose
            {
                get { return _countMatchLoose; }
                set { Set(ref _countMatchLoose, value); }
            }

            public int SumMarkedPoint
            {
                get { return _sumMarkedPoint; }
                set { Set(ref _sumMarkedPoint, value); }
            }

            public int SumTakePoint
            {
                get { return _sumTakePoint; }
                set { Set(ref _sumTakePoint, value); }
            }

            public int DifPointTakePoint
            {
                get { return _difPointTakePoint; }
                set { Set(ref _difPointTakePoint, value); }
            }

            public void RefreshVM(object sender)
            {
                CountMatchPlayed = MatchList.Count(item => item.IsFinished);
                CountMatchWin = MatchList.Count(item => item.IsFinished && item.Winner == CurrenTeam);
                CountMatchLoose = MatchList.Count(item => item.IsFinished && item.Winner != CurrenTeam);
                SumMarkedPoint = MatchList.Where(item => item.IsFinished)
                                          .Select(item => (item.Team1 == CurrenTeam) ? item.TeamScore1 : item.TeamScore2)
                                          .Sum(item => item);
                SumTakePoint = MatchList.Where(item => item.IsFinished)
                                            .Select(item => (item.Team1 == CurrenTeam) ? item.TeamScore2 : item.TeamScore1)
                                            .Sum(item => item);
                DifPointTakePoint = SumMarkedPoint - SumTakePoint;
            }
        }
    }
}
