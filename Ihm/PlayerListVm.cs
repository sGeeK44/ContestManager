using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Ihm
{
    public class PlayerListVm : ViewModel
    {
        #region Fields

        private RegisterPlayer _registerPlayerWindows;
        private ObservableCollection<IPerson> _playerList;
        private readonly IContest _currentContest;

        #endregion

        #region Constructors

        public PlayerListVm(IContest contest)
        {
            _currentContest = contest ?? throw new ArgumentNullException(nameof(contest));
            PlayerList = new ObservableCollection<IPerson>(_currentContest.TeamList.SelectMany(team => team.Members));
            SelectedPlayerList = new ObservableCollection<IPerson>();

            #region Init Commands

            UpdatePlayer = new RelayCommand(
                delegate
                    {
                        DoAddOrUpdatePlayer(SelectedPlayer);
                    },
                delegate { return SelectedPlayer != null; });

            AddPlayer = new RelayCommand(
                delegate { DoAddOrUpdatePlayer(); },
                delegate { return true; });

            RemovePlayer = new RelayCommand(
                delegate
                {
                    IPerson personToRemove = SelectedPlayer;
                    var teamAffected = personToRemove.AffectedTeam;
                    if (teamAffected != null)
                    {
                        teamAffected.RemovePlayer(personToRemove);
                        if (teamAffected.Members.Count == 0) _currentContest.UnRegister(teamAffected);
                    }
                    PlayerList.Remove(personToRemove);
                },
                delegate { return SelectedPlayer != null; });
            AddTeam = new RelayCommand(
                delegate
                {
                    var addTeamMemberWindows = new AddTeamMember(_currentContest.TeamList.Where(_ => _.Members.Count < _currentContest.MaximumPlayerByTeam).ToList());
                    addTeamMemberWindows.Show();
                    if (addTeamMemberWindows.DataContext is AddTeamMemberVm viewModel)
                    {
                        viewModel.RequestClose += o =>
                        {
                            if (o is Team selectedTeam)
                            {
                                foreach (Person person in SelectedPlayerList)
                                {
                                    selectedTeam.AddPlayer(person);
                                }
                                PlayerList = new ObservableCollection<IPerson>(PlayerList);
                            }
                            addTeamMemberWindows = null;
                        };
                    }
                },
                delegate
                {
                    return !_currentContest.IsStarted
                        && SelectedPlayer != null
                        && SelectedPlayerList.Count != 0
                        && SelectedPlayerList.Cast<Person>()
                                             .Count(item => item.AffectedTeam != null) == 0
                        //One team at less has created
                        && PlayerList.Select(item => item.AffectedTeam).Count(team => team != null) != 0;
                });
            RemoveTeam = new RelayCommand(
                delegate
                {
                    ITeam team = null;
                    foreach (IPerson person in SelectedPlayerList)
                    {
                        if (team == null) team = person.AffectedTeam;
                        team.RemovePlayer(person);
                    }
                    if (team != null && team.Members.Count == 0) _currentContest.UnRegister(team);
                    PlayerList = new ObservableCollection<IPerson>(PlayerList);
                },
                delegate
                {
                    if (SelectedPlayerList.Count == 0) return false;

                    IPerson first = null;
                    foreach (IPerson person in SelectedPlayerList)
                    {
                        if (first == null && person.AffectedTeam == null) return false;
                        if (first == null) first = person;
                        else if (first.AffectedTeam != person.AffectedTeam) return false;
                    }
                    return true;
                });
            CreateTeam = new RelayCommand(
                delegate
                {
                    var registerTeamWindows = new RegisterTeam(_currentContest);
                    registerTeamWindows.Show();
                    if (registerTeamWindows.DataContext is RegisterTeamVm viewModel)
                    {
                        viewModel.RequestClose += o =>
                        {
                            if (o != null)
                            {
                                if (o is Team newTeam)
                                {
                                    foreach (var person in SelectedPlayerList.Cast<Person>())
                                    {
                                        newTeam.AddPlayer(person);
                                    }
                                    PlayerList = new ObservableCollection<IPerson>(PlayerList);
                                }
                            }
                            registerTeamWindows = null;
                        };
                    }
                },
                delegate
                {
                    return SelectedPlayer != null
                        && SelectedPlayerList.Cast<Person>().Count(item => item.AffectedTeam != null) == 0;
                });

            #endregion
        }

        #endregion

        #region Properties

        #region Data

        public ObservableCollection<IPerson> PlayerList
        {
            get => _playerList;
            set => Set(ref _playerList, value);
        }

        public Person SelectedPlayer { get; set; }
        public IList SelectedPlayerList { get; set; }

        #endregion

        #region Commands

        public RelayCommand SaveRegisterPlayer { get; set; }
        public RelayCommand LoadRegisterPlayer { get; set; }
        public RelayCommand AddPlayer { get; set; }
        public RelayCommand UpdatePlayer { get; set; }
        public RelayCommand RemovePlayer { get; set; }
        public RelayCommand AddTeam { get; set; }
        public RelayCommand CreateTeam { get; set; }
        public RelayCommand RemoveTeam { get; set; }

        #endregion

        #endregion

        #region Methods

        private void DoAddOrUpdatePlayer(Person playerToUpdate = null)
        {
            _registerPlayerWindows = new RegisterPlayer(playerToUpdate);
            _registerPlayerWindows.Show();
            if (_registerPlayerWindows.DataContext is RegisterPlayerVm viewModel)
            {
                viewModel.RequestClose += o =>
                {
                    if (o is Person newPlayer)
                    {
                        if (PlayerList.Count(item => item.Id == newPlayer.Id) == 0)
                        {
                            PlayerList.Add(newPlayer);
                        }
                    }
                    _registerPlayerWindows = null;
                };
            }
        }

        #endregion
    }
}
