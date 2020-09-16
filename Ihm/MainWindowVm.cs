using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Service;
using SmartWay.Orm;
using SmartWay.Orm.Interfaces;

namespace Contest.Ihm
{
    public class MainWindowVm : ViewModel
    {
        #region Fields

        private PhaseViewer _phaseViewerWindows;
        private IContest _currentContest;
        private PlayerListVm _playerListVm;
        private ContestViewVm _contestVm;
        private ManagePhaseVm _manageQualificationPhaseVm;
        private ManagePhaseVm _manageMainPhaseVm;
        private ManagePhaseVm _manageConsolingPhaseVm;
        private int _indexTabSelected;

        #endregion

        #region MEF Import

        [Import]
        private IDataStore DataStore { get; set; }

        #endregion

        public MainWindowVm()
        {
            FlippingContainer.Instance.ComposeParts(this);
            DataStore.CreateOrUpdateStore();

            Exit = new RelayCommand(
                delegate
                {
                    if (CurrentContest != null)
                    {
                        CloseContest.Execute(null);
                    }
                    Application.Current.Shutdown();
                },
                delegate { return true; });

            #region Contest command

            CreateContest = new RelayCommand(
                delegate
                {
                    var contestCreateWindows = new ContestCreate();
                    contestCreateWindows.Show();
                    if (contestCreateWindows.DataContext is ContestCreateVm viewModel)
                    {
                        viewModel.RequestClose += o =>
                        {
                            if (o != null)
                            {
                                CurrentContest = o as IContest;
                                DisplayContest();
                            }
                            contestCreateWindows = null;
                        };
                    }
                },
                delegate { return CurrentContest == null; });

            OpenContest = new RelayCommand(
                delegate
                {
                    var contestSelectWindows = new ContestSelect();
                    contestSelectWindows.Show();
                    if (contestSelectWindows.DataContext is ContestSelectVm viewModel)
                    {
                        viewModel.RequestClose += o =>
                        {
                            if (o is IContest selected)
                            {
                                CurrentContest = selected;
                                DisplayContest();
                            }
                            contestSelectWindows = null;
                        };
                    }
                },
                delegate { return CurrentContest == null; });

            CloseContest = new RelayCommand(
                delegate
                {
                    _phaseViewerWindows?.Close();
                    CurrentContest = null;
                    PlayerListVm = null;
                    ContestVm = null;
                    ManageQualificationPhaseVm = null;
                    ManageMainPhaseVm = null;
                    ManageConsolingPhaseVm = null;
                    IndexTabSelected = 0;
                    RefreshVm();
                },
                delegate { return CurrentContest != null; });
            StartContest = new RelayCommand(
                delegate
                {
                    ContestVm.UpdateContest();
                    var contestService = new ContestService();
                    contestService.StartContest(CurrentContest);
                    DisplayContest();
                },
                delegate
                {
                    if (CurrentContest == null || CurrentContest.IsStarted) return false;
                    //Ensure field is set
                    if (CurrentContest.CountField == 0) return false;

                    //Ensure min team
                    if (string.Equals(ContestVm.CountMinTeamRegister, ContestViewVm.NotANumber)) return false;
                    if (CurrentContest.TeamList.Count < int.Parse(ContestVm.CountMinTeamRegister)) return false;

                    //Ensure max team
                    if (string.Equals(ContestVm.CountMaxTeamRegister, ContestViewVm.NotANumber)) return false;
                    if (!string.Equals(ContestVm.CountMaxTeamRegister, ContestViewVm.Infinit)
                     && CurrentContest.TeamList.Count > int.Parse(ContestVm.CountMaxTeamRegister)) return false;

                    //Ensure min and max player by team
                    if (CurrentContest.TeamList.Count(_ => _.Members.Count < CurrentContest.MinimumPlayerByTeam
                                                        || _.Members.Count > CurrentContest.MaximumPlayerByTeam) != 0) return false;

                    return true;
                });

            ShowPhaseViewer = new RelayCommand(
                delegate
                {
                    _phaseViewerWindows = new PhaseViewer(_currentContest);
                    _phaseViewerWindows.Show();
                },
                delegate
                {
                    return CurrentContest != null
                        && CurrentContest.IsStarted;
                });

            StartNextPhase = new RelayCommand(
                delegate
                {
                    CurrentContest.LaunchNextPhase();
                    var contestService = new ContestService();
                    contestService.SaveContest(CurrentContest);
                    RefreshVm();
                },
                delegate
                {
                    return CurrentContest != null
                        && CurrentContest.WithQualificationPhase
                        && CurrentContest.QualificationPhase != null
                        && CurrentContest.QualificationPhase.GameStepList.Count(_ => !_.IsMatchListComplete) == 0
                        && !CurrentContest.QualificationPhase.IsFinished;
                });

            EndContest = new RelayCommand(
                delegate
                {
                    CurrentContest.LaunchNextPhase();
                    RefreshVm();
                },
                delegate
                {
                    return CurrentContest != null
                        && !CurrentContest.IsStarted
                        && CurrentContest.PhaseList.Count(_ => !_.IsFinished) != 0
                        && !CurrentContest.IsFinished;
                });

            #endregion
        }

        private void DisplayContest()
        {
            ContestVm = new ContestViewVm(CurrentContest);
            PlayerListVm = new PlayerListVm(CurrentContest);
            if (!CurrentContest.IsStarted)
            {
                IndexTabSelected = 1;
            }
            else
            {
                foreach (var phase in _currentContest.PhaseList)
                {
                    switch (phase.Type)
                    {
                        case PhaseType.Qualification:
                            ManageQualificationPhaseVm = new ManagePhaseVm(phase, _currentContest.FieldList);
                            break;
                        case PhaseType.Main:
                            ManageMainPhaseVm = new ManagePhaseVm(phase, _currentContest.FieldList);
                            break;
                        case PhaseType.Consoling:
                            ManageConsolingPhaseVm = new ManagePhaseVm(phase, _currentContest.FieldList);
                            break;
                    }
                }

                _currentContest.NewPhaseLaunch += (sender, phase) =>
                {
                    if (phase == null) return;
                    switch (phase.Type)
                    {
                        case PhaseType.Main:
                            ManageMainPhaseVm = new ManagePhaseVm(phase, _currentContest.FieldList);
                            break;
                        case PhaseType.Consoling:
                            ManageConsolingPhaseVm = new ManagePhaseVm(phase, _currentContest.FieldList);
                            break;
                    }

                };
                IndexTabSelected = CurrentContest.WithQualificationPhase ? 2 : 3;
            }
            RefreshVm();
        }

        private void RefreshVm()
        {
            OnPropertyChanged(() => IsContestStarted);
            OnPropertyChanged(() => HasQualification);
            OnPropertyChanged(() => ShowMain);
            OnPropertyChanged(() => EnableMain);
            OnPropertyChanged(() => ShowQualification);
            OnPropertyChanged(() => EnableQualification);
            OnPropertyChanged(() => ShowConsoling);
            OnPropertyChanged(() => EnableConsoling);
            OnPropertyChanged(() => ShowContest);
        }

        public IContest CurrentContest
        {
            get => _currentContest;
            set => Set(ref _currentContest, value);
        }

        public bool HasQualification => _currentContest != null && _currentContest.WithQualificationPhase;

        public bool IsContestStarted => CurrentContest != null && _currentContest.IsStarted;

        public ContestViewVm ContestVm
        {
            get => _contestVm;
            set => Set(ref _contestVm, value);
        }

        public PlayerListVm PlayerListVm
        {
            get => _playerListVm;
            set => Set(ref _playerListVm, value);
        }

        public RelayCommand CreateContest { get; set; }
        public RelayCommand OpenContest { get; set; }
        public RelayCommand CloseContest { get; set; }
        public RelayCommand Exit { get; set; }

        public RelayCommand StartContest { get; set; }
        public RelayCommand ShowPhaseViewer { get; set; }
        public RelayCommand StartNextPhase { get; set; }
        public RelayCommand EndContest { get; set; }

        public ManagePhaseVm ManageQualificationPhaseVm
        {
            get => _manageQualificationPhaseVm;
            set => Set(ref _manageQualificationPhaseVm, value);
        }

        public ManagePhaseVm ManageMainPhaseVm
        {
            get => _manageMainPhaseVm;
            set => Set(ref _manageMainPhaseVm, value);
        }

        public ManagePhaseVm ManageConsolingPhaseVm
        {
            get => _manageConsolingPhaseVm;
            set => Set(ref _manageConsolingPhaseVm, value);
        }

        public bool ShowMain =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.PrincipalPhase != null
            && CurrentContest.PrincipalPhase.IsStarted;

        public bool EnableMain =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.PrincipalPhase != null
            && CurrentContest.PrincipalPhase.IsStarted;

        public bool ShowQualification =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.WithQualificationPhase;

        public bool EnableQualification =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.WithQualificationPhase
            && CurrentContest.QualificationPhase.IsStarted;

        public bool ShowConsoling =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.WithConsolante;

        public bool EnableConsoling =>
            CurrentContest != null
            && CurrentContest.IsStarted
            && CurrentContest.WithConsolante
            && CurrentContest.ConsolingPhase != null
            && CurrentContest.ConsolingPhase.IsStarted;

        public bool ShowContest => CurrentContest != null;

        public int IndexTabSelected
        {
            get => _indexTabSelected;
            set => Set( ref _indexTabSelected, value);
        }
    }
}
