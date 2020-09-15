using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;
using Contest.Service;

namespace Contest.Ihm
{
    public class PersonVm : ViewModel
    {
        private readonly IContest _contest;

        public PersonVm(IContest contest, IPerson person)
        {
            _contest = contest;
            Person = person;
        }

        public IPerson Person { get; }

        public ITeam AffectedTeam => Person.GetTeam(_contest);

        public string Alias => Person.Alias;

        public string LastName => Person.LastName;

        public string FirstName => Person.FirstName;

        public string Mail => Person.Mail;
    }
    public class PlayerListVm : ViewModel
    {
        #region MEF Import

        [Import] private IRepository<Person, IPerson> PersonRepository { get; set; }

        #endregion

        #region Fields

        private ObservableCollection<PersonVm> _playerList;

        #endregion

        #region Constructors

        public PlayerListVm(IContest contest)
        {
            FlippingContainer.Instance.ComposeParts(this);
            Contest = contest ?? throw new ArgumentNullException(nameof(contest));
            RefreshPlayerList(null, null);
        }

        #endregion

        #region Properties

        #region Data

        public IContest Contest { get; set; }

        public ObservableCollection<PersonVm> PlayerList
        {
            get => _playerList;
            set => Set(ref _playerList, value);
        }

        public PersonVm SelectedPlayer { get; set; }

        #endregion

        #region Commands

        public RelayCommand AddPlayer => new RelayCommand(_ =>
            {
                DisplayRegisterPlayer();
            },
            delegate { return true; }
        );

        public RelayCommand UpdatePlayer => new RelayCommand(_ =>
            {
                DisplayRegisterPlayer(SelectedPlayer);
            },
            delegate { return SelectedPlayer != null; }
        );

        private void DisplayRegisterPlayer(PersonVm player = null)
        {
            var registerPlayervm = new RegisterPlayerVm(player?.Person);
            var registerPlayerWindows = new RegisterPlayer { DataContext = registerPlayervm };
            registerPlayerWindows.Show();
            registerPlayerWindows.Closed += RefreshPlayerList;
        }

        private void RefreshPlayerList(object sender, EventArgs e)
        {
            PlayerList = new ObservableCollection<PersonVm>(PersonRepository.GetAll().Select(person => new PersonVm(Contest, person)));
        }

        public RelayCommand RemovePlayer => new RelayCommand(_ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                var service = new ContestService();
                service.RemovePerson(Contest, selectedPerson.Select(vm => vm.Person).ToList());
                RefreshPlayerList(null, null);
            },
            delegate { return SelectedPlayer != null; }
        );

        public RelayCommand AddTeam => new RelayCommand(_ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                var addMemberMemberData = new AddTeamMemberVm(Contest, selectedPerson.Select(vm => vm.Person).ToList());
                var addTeamMemberWindows = new AddTeamMember { DataContext = addMemberMemberData };
                addTeamMemberWindows.Show();
                addTeamMemberWindows.Closed += RefreshPlayerList;
            },
            _ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                return !Contest.IsStarted
                       && selectedPerson.Count != 0
                       && selectedPerson.Count(item => item.AffectedTeam != null) == 0;
            });
        public RelayCommand CreateTeam => new RelayCommand(_ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                var registerTeamVm = new RegisterTeamVm(Contest, selectedPerson.Select(vm => vm.Person).ToList());
                var registerTeamWindows = new RegisterTeam { DataContext = registerTeamVm };
                registerTeamWindows.Show();
                registerTeamWindows.Closed += RefreshPlayerList;
            },
            _ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                return selectedPerson.All(item => item.AffectedTeam == null);
            }
        );

        public RelayCommand RemoveTeam => new RelayCommand(_ =>
            {
                var selectedPerson = ((IList) _).Cast<PersonVm>().ToList();
                var service = new ContestService();
                service.RemovePlayer(Contest, selectedPerson.Select(vm => vm.Person).ToList());
                RefreshPlayerList(null, null);
            },
            _ =>
            {
                var selectedPerson = ((IList)_).Cast<PersonVm>().ToList();
                return selectedPerson.Count != 0 && selectedPerson.Any(_ => _.AffectedTeam != null);
            }
        );

        #endregion

        #endregion
    }
}
