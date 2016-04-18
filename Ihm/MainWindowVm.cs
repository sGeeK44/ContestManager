using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using Contest.Business;
using Contest.Core.Component;
using Contest.Core.DataStore.Sqlite;
using Contest.Core.Repository.Sql;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

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

        private ISqlUnitOfWorks UnitOfWorks { get; set; }

        [Import]
        private ISqlRepository<IContest> ContestRepository { get; set; }

        [Import]
        private ISqlRepository<IGameSetting> GameSettingRepository { get; set; }

        [Import]
        private ISqlRepository<IAddress> AddressRepository { get; set; }

        [Import]
        private ISqlRepository<IPhysicalSetting> PhysicalSettingRepository { get; set; }

        [Import]
        private ISqlRepository<IEliminationStepSetting> EliminationStepSettingRepository { get; set; }

        [Import]
        private ISqlRepository<IQualificationStepSetting> QualificationStepSettingRepository { get; set; }

        [Import]
        private ISqlRepository<IMatchSetting> MatchSettingRepository { get; set; }

        [Import]
        private ISqlRepository<ITeam> TeamRepository { get; set; }

        [Import]
        private ISqlRepository<IMatch> MatchRepository { get; set; }

        [Import]
        private ISqlRepository<IPerson> PersonRepository { get; set; }

        [Import]
        private ISqlRepository<IPhase> PhaseRepository { get; set; }

        [Import]
        private ISqlRepository<IEliminationStep> EliminationStepRepository { get; set; }

        [Import]
        private ISqlRepository<IQualificationStep> QualificationStepRepository { get; set; }

        [Import]
        private ISqlRepository<IField> FieldRepository { get; set; }

        [Import]
        private ISqlRepository<IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import]
        private ISqlRepository<IRelationship<ITeam, IPhase>>  TeamPhaseRelationship { get; set; }

        #endregion

        public MainWindowVm()
        {
            FlippingContainer.Instance.ComposeParts(this);

            var dataStore = new SqliteDataStore(ConfigurationManager.AppSettings["DatabasePath"]);
            dataStore.OpenDatabase();

            UnitOfWorks = new SqlUnitOfWorks(dataStore);
            UnitOfWorks.AddRepository(ContestRepository);
            UnitOfWorks.AddRepository(GameSettingRepository);
            UnitOfWorks.AddRepository(AddressRepository);
            UnitOfWorks.AddRepository(PhysicalSettingRepository);
            UnitOfWorks.AddRepository(EliminationStepSettingRepository);
            UnitOfWorks.AddRepository(QualificationStepSettingRepository);
            UnitOfWorks.AddRepository(MatchSettingRepository);
            UnitOfWorks.AddRepository(TeamRepository);
            UnitOfWorks.AddRepository(MatchRepository);
            UnitOfWorks.AddRepository(PersonRepository);
            UnitOfWorks.AddRepository(PhaseRepository);
            UnitOfWorks.AddRepository(EliminationStepRepository);
            UnitOfWorks.AddRepository(QualificationStepRepository);
            UnitOfWorks.AddRepository(FieldRepository);
            UnitOfWorks.AddRepository(TeamGameStepRelationshipRepository);
            UnitOfWorks.AddRepository(TeamPhaseRelationship);

            if (!File.Exists(ConfigurationManager.AppSettings["DatabasePath"]))
            {
                ContestRepository.CreateTable();
                GameSettingRepository.CreateTable();
                AddressRepository.CreateTable();
                PhysicalSettingRepository.CreateTable();
                EliminationStepSettingRepository.CreateTable();
                QualificationStepSettingRepository.CreateTable();
                MatchSettingRepository.CreateTable();
                TeamRepository.CreateTable();
                MatchRepository.CreateTable();
                PersonRepository.CreateTable();
                PhaseRepository.CreateTable();
                EliminationStepRepository.CreateTable();
                QualificationStepRepository.CreateTable();
                FieldRepository.CreateTable();
                TeamGameStepRelationshipRepository.CreateTable();
                TeamPhaseRelationship.CreateTable();
                UnitOfWorks.Commit();
            }

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
                    var viewModel = contestCreateWindows.DataContext as ContestCreateVm;
                    if (viewModel != null)
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
                    var viewModel = contestSelectWindows.DataContext as ContestSelectVm;
                    if (viewModel != null)
                    {
                        viewModel.RequestClose += o =>
                        {
                            var selected = o as IContest;
                            if (selected != null)
                            {
                                CurrentContest = selected;
                                DisplayContest();
                            }
                            contestSelectWindows = null;
                        };
                    }
                },
                delegate { return CurrentContest == null; });

            SaveContest = new RelayCommand(
                delegate { Save(); },
                delegate { return CurrentContest != null && UnitOfWorks.IsBinded; });

            CloseContest = new RelayCommand(
                delegate
                {
                    var result = MessageBox.Show("Voulez-vous sauvegarder la progression du tournoi ?", "Contest", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes) Save();

                    if (_phaseViewerWindows != null) _phaseViewerWindows.Close();
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
                    CurrentContest.StartContest();
                    DisplayContest();
                },
                delegate
                {
                    if (CurrentContest == null || CurrentContest.IsStarted) return false;
                    //Ensure field is set
                    if (CurrentContest.CountField == 0) return false;

                    //Ensure min team
                    if (string.Equals(ContestVm.CountMinTeamRegister, ContestViewVm.NOT_A_NUMBER)) return false;
                    if (CurrentContest.TeamList.Count < int.Parse(ContestVm.CountMinTeamRegister)) return false;

                    //Ensure max team
                    if (string.Equals(ContestVm.CountMaxTeamRegister, ContestViewVm.NOT_A_NUMBER)) return false;
                    if (!string.Equals(ContestVm.CountMaxTeamRegister, ContestViewVm.INFINIT)
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

        internal void Save()
        {
            if (CurrentContest == null) return;
            if (!CurrentContest.IsStarted) ContestVm.UpdateContest();
            CurrentContest.PrepareCommit(UnitOfWorks);
            UnitOfWorks.Commit();
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
            get { return _currentContest; }
            set { Set(ref _currentContest, value); }
        }

        public bool HasQualification
        {
            get
            {
                return _currentContest != null && _currentContest.WithQualificationPhase;
            }
        }

        public bool IsContestStarted
        {
            get { return CurrentContest != null && _currentContest.IsStarted; }
        }

        public ContestViewVm ContestVm
        {
            get { return _contestVm; }
            set
            {
                Set(ref _contestVm, value);
            }
        }

        public PlayerListVm PlayerListVm
        {
            get { return _playerListVm; }
            set
            {
                Set(ref _playerListVm, value);
            }
        }

        public RelayCommand CreateContest { get; set; }
        public RelayCommand OpenContest { get; set; }
        public RelayCommand SaveContest { get; set; }
        public RelayCommand SaveAsContest { get; set; }
        public RelayCommand CloseContest { get; set; }
        public RelayCommand Exit { get; set; }

        public RelayCommand StartContest { get; set; }
        public RelayCommand UpdateContest { get; set; }
        public RelayCommand ShowPhaseViewer { get; set; }
        public RelayCommand StartNextPhase { get; set; }
        public RelayCommand EndContest { get; set; }

        public ManagePhaseVm ManageQualificationPhaseVm
        {
            get { return _manageQualificationPhaseVm; }
            set { Set(ref _manageQualificationPhaseVm, value); }
        }

        public ManagePhaseVm ManageMainPhaseVm
        {
            get { return _manageMainPhaseVm; }
            set { Set(ref _manageMainPhaseVm, value); }
        }

        public ManagePhaseVm ManageConsolingPhaseVm
        {
            get { return _manageConsolingPhaseVm; }
            set { Set(ref _manageConsolingPhaseVm, value); }
        }

        public bool ShowMain
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.PrincipalPhase != null
                    && CurrentContest.PrincipalPhase.IsStarted;
            }
        }

        public bool EnableMain
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.PrincipalPhase != null
                    && CurrentContest.PrincipalPhase.IsStarted;
            }
        }

        public bool ShowQualification
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.WithQualificationPhase;
            }
        }

        public bool EnableQualification
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.WithQualificationPhase
                    && CurrentContest.QualificationPhase.IsStarted;
            }
        }

        public bool ShowConsoling
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.WithConsolante;
            }
        }

        public bool EnableConsoling
        {
            get
            {
                return CurrentContest != null
                    && CurrentContest.IsStarted
                    && CurrentContest.WithConsolante
                    && CurrentContest.ConsolingPhase != null
                    && CurrentContest.ConsolingPhase.IsStarted;
            }
        }

        public bool ShowContest
        {
            get { return CurrentContest != null; }
        }

        public int IndexTabSelected
        {
            get { return _indexTabSelected; }
            set { Set( ref _indexTabSelected, value); }
        }
    }
}
