using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using Contest.Domain.Settings;

namespace Contest.Ihm
{
    public class ManagePhaseVm : ViewModel
    {
        private int _countTotalMatch;

        public ManagePhaseVm(IPhase current, IList<IField> fieldList)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            GameStepList = new ObservableCollection<GameStepVm>();
            NextGameList = new ObservableCollection<MatchUpdateVm>();
            FinishedGameList = new ObservableCollection<MatchUpdateVm>();
            foreach (var gameStep in current.GameStepList
                                             .Where(item => !item.IsFinished)
                                             .Select(item => new GameStepVm(item, fieldList)))
            {
                GameStepList.Add(gameStep);
            }

            var allSortedMatch = SortedMatch(current);
            CountTotalMatch = allSortedMatch.Count;
            foreach (var match in allSortedMatch)
            {
                var matchVm = new MatchUpdateVm(match, fieldList);
                if (!match.IsBeginning) NextGameList.Add(matchVm);
                if (match.IsFinished) FinishedGameList.Add(matchVm);
                match.MatchEnded += sender =>
                {
                    FinishedGameList.Add(matchVm);
                    OnPropertyChanged(() => HasMatchEnded);
                };
                match.MatchStarted += sender =>
                {
                    NextGameList.Remove(matchVm);
                    OnPropertyChanged(() => HasMatchPlanned);
                };
            }

            current.NextStepStarted += (sender, gameStep) =>
            {
                GameStepList.Add(new GameStepVm(gameStep, fieldList));
                foreach (var match in SortedMatch(new List<IGameStep>{ gameStep }, current.TeamList))
                {
                    var matchVm = new MatchUpdateVm(match, fieldList);
                    if (!match.IsBeginning) NextGameList.Add(matchVm);
                    if (match.IsFinished) FinishedGameList.Add(matchVm);
                    match.MatchEnded += s =>
                    {
                        FinishedGameList.Add(matchVm);
                        OnPropertyChanged(() => HasMatchEnded);
                    };
                    match.MatchStarted += s =>
                    {
                        NextGameList.Remove(matchVm);
                        OnPropertyChanged(() => HasMatchPlanned);
                    };
                }
                OnPropertyChanged(() => HasMatchPlanned);
            };
        }

        public static IList<IMatch> SortedMatch(IPhase current)
        {
            return SortedMatch(current.GameStepList, current.TeamList);
        }

        public static IList<IMatch> SortedMatch(IList<IGameStep> gameStepList, IList<ITeam> teamList)
        {
            IList<IList<IMatch>> gameStepSortedMatch = new List<IList<IMatch>>();
            var countTotal = 0;
            int i;
            foreach (var gameStep in gameStepList)
            {
                IList<IMatch> sortedGameStep = new List<IMatch>();
                IList<IMatch> allMatchGameStep = new List<IMatch>(gameStep.MatchList);
                i = 0;
                var max = allMatchGameStep.Count;
                countTotal += max;
                while (i < max)
                {
                    var added = new List<ITeam>();
                    foreach (var team in teamList)
                    {
                        var matchToAdd = allMatchGameStep.FirstOrDefault(item => item.IsTeamInvolved(team) && added.Find(_ => _ == item.Team1 || _ == item.Team2) == null);
                        if (matchToAdd == null) continue;
                        added.Add(matchToAdd.Team1);
                        added.Add(matchToAdd.Team2);
                        sortedGameStep.Add(matchToAdd);
                        allMatchGameStep.Remove(matchToAdd);
                        i++;
                    }
                    //To manage unpair team list.
                    if (added.Count != teamList.Count)
                    {
                        var teamWithoutMatch = teamList.FirstOrDefault(_ => !added.Contains(_));
                        var matchToAdd = allMatchGameStep.FirstOrDefault(item => item.IsTeamInvolved(teamWithoutMatch));
                        if (matchToAdd == null) continue;
                        sortedGameStep.Add(matchToAdd);
                        allMatchGameStep.Remove(matchToAdd);
                        i++;
                    }
                }
                gameStepSortedMatch.Add(sortedGameStep);
            }

            IList<IMatch> sorted = new List<IMatch>();
            i = 0;
            while (i < countTotal)
            {
                foreach (var matchList in gameStepSortedMatch)
                {
                    var first = matchList.FirstOrDefault();
                    if (first == null) continue;
                    sorted.Add(first);
                    matchList.Remove(first);
                    i++;
                }
            }
            return sorted;
        }

        public ObservableCollection<GameStepVm> GameStepList { get; set; }

        public ObservableCollection<MatchUpdateVm> NextGameList { get; set; }

        public ObservableCollection<MatchUpdateVm> FinishedGameList { get; set; }

        public bool HasMatchEnded => FinishedGameList.Count != 0;

        public bool HasMatchPlanned => NextGameList.Count != 0;

        public int CountTotalMatch
        {
            get => _countTotalMatch;
            set => Set(ref _countTotalMatch, value);
        }
    }

    public class GameStepVm : ViewModel
    {
        private string _name;
        private ObservableCollection<MatchUpdateVm> _matchList;
        private bool _hasNext;

        public GameStepVm(IGameStep current, IList<IField> fieldList)
        {
            Name = current.Name;
            ShowNextStepButton = current.NextStep != null;
            MatchList = new ObservableCollection<MatchUpdateVm>();
            foreach (var match in current.MatchList)
            {
                var matchVm = new MatchUpdateVm(match, fieldList);
                if (match.IsBeginning && !match.IsFinished) MatchList.Add(matchVm);
                match.MatchEnded += sender =>
                {
                    MatchList.Remove(matchVm);
                    OnPropertyChanged(() => HasMatchInProgress);
                };
                match.MatchStarted += sender =>
                {
                    MatchList.Add(matchVm);
                    OnPropertyChanged(() => HasMatchInProgress);
                    MessageBox.Show("Match démarré sur le terrain " + match.MatchField.Name);
                };
            }
            LaunchNextStep = new RelayCommand(
                delegate
                {
                    current.EndGameStep();
                    current.Phase.LaunchNextStep();
                    ShowNextStepButton = false;
                },
                delegate
                {
                    return current.IsMatchListComplete;
                });
        }

        public bool ShowNextStepButton
        {
            get => _hasNext;
            set => Set(ref _hasNext, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ObservableCollection<MatchUpdateVm> MatchList
        {
            get => _matchList;
            set
            {
                Set( ref _matchList, value);
                OnPropertyChanged(()=>HasMatchInProgress);
            }
        }

        public bool HasMatchInProgress => _matchList.Count != 0;
        public RelayCommand LaunchNextStep { get; set; }
    }
}
