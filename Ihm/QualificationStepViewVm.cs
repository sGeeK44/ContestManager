using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using Contest.Domain.Players;

namespace Contest.Ihm
{
    public class QualificationStepViewVm : ViewModel
    {
        private readonly IQualificationStep _step;

        public QualificationStepViewVm(IQualificationStep step)
        {
            GroupName = step.Name;
            _step = step;
            _step.RankChanged += sender => OnPropertyChanged(() => RowList);
        }

        public string GroupName { get; set; }

        public List<RowVm> RowList
        {
            get
            {
                return _step.Rank.Select(team => new RowVm(team, _step.MatchList.Where(item => item.IsTeamInvolved(team)).ToList()))
                                 .ToList();
            }
        }

        public class RowVm : ViewModel
        {
            private int _countMatchPlayed;
            private int _countMatchWin;
            private int _countMatchLoose;
            private int _sumMarkedPoint;
            private int _sumTakePoint;
            private int _difPointTakePoint;

            public RowVm(ITeam team, IList<IMatch> matchPlayed)
            {
                if (matchPlayed == null) throw new ArgumentNullException(nameof(matchPlayed));

                CurrenTeam = team ?? throw new ArgumentNullException(nameof(team));
                Name = team.Name;
                MatchList = new ObservableCollection<IMatch>(matchPlayed);
                foreach (var match in matchPlayed)
                {
                    match.MatchEnded += RefreshVm;
                    match.ScoreChanged += RefreshVm;
                }

                RefreshVm(null);
            }

            public ITeam CurrenTeam { get; set; }
            public ObservableCollection<IMatch> MatchList { get; set; }

            public string Name { get; set; }

            public int CountMatchPlayed
            {
                get => _countMatchPlayed;
                set => Set(ref _countMatchPlayed, value);
            }

            public int CountMatchWin
            {
                get => _countMatchWin;
                set => Set(ref _countMatchWin, value);
            }

            public int CountMatchLoose
            {
                get => _countMatchLoose;
                set => Set(ref _countMatchLoose, value);
            }

            public int SumMarkedPoint
            {
                get => _sumMarkedPoint;
                set => Set(ref _sumMarkedPoint, value);
            }

            public int SumTakePoint
            {
                get => _sumTakePoint;
                set => Set(ref _sumTakePoint, value);
            }

            public int DifPointTakePoint
            {
                get => _difPointTakePoint;
                set => Set(ref _difPointTakePoint, value);
            }

            public void RefreshVm(object sender)
            {
                CountMatchPlayed = MatchList.Count(item => item.IsFinished);
                CountMatchWin = MatchList.Count(item => item.IsFinished && item.Winner.Equals(CurrenTeam));
                CountMatchLoose = MatchList.Count(item => item.IsFinished && !item.Winner.Equals(CurrenTeam));
                SumMarkedPoint = MatchList.Where(item => item.IsFinished)
                                          .Select(item => (item.Team1.Equals(CurrenTeam)) ? item.TeamScore1 : item.TeamScore2)
                                          .Sum(item => item);
                SumTakePoint = MatchList.Where(item => item.IsFinished)
                                            .Select(item => (item.Team1.Equals(CurrenTeam)) ? item.TeamScore2 : item.TeamScore1)
                                            .Sum(item => item);
                DifPointTakePoint = SumMarkedPoint - SumTakePoint;
            }
        }
    }
}
