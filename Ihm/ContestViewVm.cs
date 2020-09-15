using System;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Core.Helper;
using Contest.Core.Windows.Mvvm;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Service;

namespace Contest.Ihm
{
    public class ContestViewVm : ViewModel
    {
        public const string NotANumber = "N/A";
        public const string Infinit = "++";
        private EliminationType _firstEliminationStep;
        private EliminationType? _firstConsolingEliminationStep;
        private bool _hasQualificationStep;
        private bool _hasConsolingStep;
        private ushort? _countQualificationGroup;
        private ushort? _countQualifiedTeam;
        private int? _countFishPlayer;
        private bool _withRevenge;
        private MatchSettingVm _eliminationSettingVm;
        private MatchSettingVm _consolingEliminationSettingVm;
        private MatchSettingVm _qualificationSettingVm;
        private IContest _currentContest;

        #region Constructors

        public ContestViewVm(IContest contest)
        {
            if (contest == null) throw new ArgumentNullException(nameof(contest));
            if (contest.EliminationSetting == null) throw new InvalidProgramException("Elimination setting can not be null.");
            
            FirstEliminationStep = contest.EliminationSetting.FirstStep;
            EliminationSettingVm = new MatchSettingVm(contest.EliminationSetting.MatchSetting);
            HasQualificationStep = contest.WithQualificationPhase;
            HasConsolingStep = contest.WithConsolante;

            if (contest.QualificationSetting != null)
            {
                CountQualificationGroup = contest.QualificationSetting.CountGroup;
                WithRevenge = contest.QualificationSetting.MatchWithRevenche;
                
                QualificationSettingVm = new MatchSettingVm(contest.QualificationSetting.MatchSetting);
            }
            else QualificationSettingVm = new MatchSettingVm();

            if (contest.ConsolingEliminationSetting != null)
            {
                FirstConsolingEliminationStep = contest.ConsolingEliminationSetting.FirstStep;
                ConsolingEliminationSettingVm = new MatchSettingVm(contest.ConsolingEliminationSetting.MatchSetting);
            }
            else ConsolingEliminationSettingVm = new MatchSettingVm();

            CurrentContest = contest;
        }

        public IContest CurrentContest
        {
            get => _currentContest;
            set => Set( ref _currentContest, value);
        }

        #endregion

        #region Properties VM

        public string CountMinTeamRegister
        {
            get
            {
                var minTeam = Domain.Games.Contest.MinTeamRegister(CountTeamFirstEliminationStep, HasQualificationStep,
                                                                CountQualificationGroup, HasConsolingStep, CountTeamFirstConsolingEliminationStep);
                return minTeam != null ? minTeam.ToString() : NotANumber;
            }
        }

        public string CountMaxTeamRegister
        {
            get
            {
                var maxTeam = Domain.Games.Contest.MaxTeamRegister(CountTeamFirstEliminationStep, HasQualificationStep, CountQualificationGroup, HasConsolingStep, CountTeamFirstConsolingEliminationStep);
                if (maxTeam == null) return NotANumber;
                return maxTeam == int.MaxValue ? Infinit : maxTeam.ToString();
            }
        }

        public uint CountMaxPlayerByTeam
        {
            get => CurrentContest.MaximumPlayerByTeam;
            set
            {
                CurrentContest.MaximumPlayerByTeam = value;
                OnPropertyChanged(()=>CountMaxPlayerByTeam);
            }
        }

        public uint CountMinPlayerByTeam
        {
            get => CurrentContest.MinimumPlayerByTeam;
            set
            {
                CurrentContest.MinimumPlayerByTeam = value;
                OnPropertyChanged(() => CountMinPlayerByTeam);
            }
        }

        public ushort CountField
        {
            get => CurrentContest.CountField;
            set
            {
                CurrentContest.CountField = value;
                OnPropertyChanged(()=>CountField);
            }
        }

        public ObservableCollection<EliminationType> AvailableEliminationStep => new ObservableCollection<EliminationType>(EnumHelper.GetValueList<EliminationType>());

        public EliminationType FirstEliminationStep
        {
            get => _firstEliminationStep;
            set
            {
                Set(ref _firstEliminationStep, value);
                SetQualificationCount(null, CountQualifiedTeam);
                OnPropertyChanged(() => CountTeamFirstEliminationStep);
                OnPropertyChanged(() => CountMinTeamRegister);
                OnPropertyChanged(() => CountMaxTeamRegister);
                OnPropertyChanged(() => AvailableConsolingEliminationStep);
                OnPropertyChanged(() => AvailableNumberOfQualificationGroup);
                OnPropertyChanged(() => AvailableNumberOfQualifiedTeam);
            }
        }

        public ushort CountTeamFirstEliminationStep => (ushort)((ushort)FirstEliminationStep * 2);

        public bool HasQualificationStep
        {
            get => _hasQualificationStep;
            set
            {
                Set(ref _hasQualificationStep, value);
                if (!value)
                {
                    HasConsolingStep = false;
                    CountQualificationGroup = null;
                    CountQualifiedTeam = null;
                    CountFishPlayer = null;
                }
                else
                {
                    OnPropertyChanged(() => CountMinTeamRegister);
                    OnPropertyChanged(() => CountMaxTeamRegister);
                }
                OnPropertyChanged(() => AvailableNumberOfQualificationGroup);
                OnPropertyChanged(() => AvailableNumberOfQualifiedTeam);
            }
        }

        public ObservableCollection<ushort> AvailableNumberOfQualificationGroup
        {
            get
            {
                var result = new ObservableCollection<ushort>();
                var max = CountTeamFirstEliminationStep;
                for (ushort i = 1; i <= max; i++)
                {
                    result.Add(i);
                }
                return result;
            }
        }

        public ObservableCollection<ushort> AvailableNumberOfQualifiedTeam
        {
            get
            {
                var result = new ObservableCollection<ushort>();
                if (AvailableNumberOfQualificationGroup.Count == 0) return result;
                var max = AvailableNumberOfQualificationGroup.Max();
                var min = AvailableNumberOfQualificationGroup.Min();
                for (ushort i = 1; i <= CountTeamFirstEliminationStep; i++)
                {
                    ushort countFishedTeam;
                    ushort countGroup;
                    QualificationStepSetting.ComputeGroup(FirstEliminationStep, i, out countGroup, out countFishedTeam);
                    if (countGroup > max || countGroup < min) continue;
                    result.Add(i);
                }
                return result;
            }
        }

        public bool HasConsolingStep
        {
            get => _hasConsolingStep;
            set
            {
                Set(ref _hasConsolingStep, value);
                if (!value) FirstConsolingEliminationStep = null;
                OnPropertyChanged(() => CountMinTeamRegister);
                OnPropertyChanged(() => CountMaxTeamRegister);
                OnPropertyChanged(() => AvailableNumberOfQualificationGroup);
                OnPropertyChanged(() => AvailableNumberOfQualifiedTeam);
            }
        }

        public EliminationType? FirstConsolingEliminationStep
        {
            get => _firstConsolingEliminationStep;
            set
            {
                Set(ref _firstConsolingEliminationStep, value);
                SetQualificationCount(CountQualificationGroup, CountQualifiedTeam);
                OnPropertyChanged(() => CountTeamFirstEliminationStep);
                OnPropertyChanged(() => CountMinTeamRegister);
                OnPropertyChanged(() => CountMaxTeamRegister);
                OnPropertyChanged(() => AvailableNumberOfQualificationGroup);
                OnPropertyChanged(() => AvailableNumberOfQualifiedTeam);
            }
        }

        public ObservableCollection<EliminationType> AvailableConsolingEliminationStep => new ObservableCollection<EliminationType>(EnumHelper.GetValueList<EliminationType>());

        public ushort? CountTeamFirstConsolingEliminationStep =>
            FirstConsolingEliminationStep != null
                ? (ushort?)((ushort)FirstConsolingEliminationStep.Value * 2)
                : null;

        public int? CountFishPlayer
        {
            get => _countFishPlayer;
            set
            {
                Set(ref _countFishPlayer, value);
                OnPropertyChanged(()=>HasFishPlayer);
            }
        }

        public bool HasFishPlayer => CountFishPlayer != null && CountFishPlayer != 0;

        public ushort? CountQualifiedTeam
        {
            get => _countQualifiedTeam;
            set => SetQualificationCount(null, value);
        }

        public ushort? CountQualificationGroup
        {
            get => _countQualificationGroup;
            set => SetQualificationCount(value, null);
        }

        private void SetQualificationCount(ushort? countQualificationGroup, ushort? countQualifiedTeam)
        {
            if (countQualificationGroup != null &&
                !AvailableNumberOfQualificationGroup.Contains(countQualificationGroup.Value))
                countQualificationGroup = null;

            if (countQualifiedTeam != null &&
                !AvailableNumberOfQualifiedTeam.Contains(countQualifiedTeam.Value))
                countQualifiedTeam = null;

            if (countQualificationGroup == null && countQualifiedTeam == null)
            {
                Set("CountQualificationGroup", ref _countQualificationGroup, null);
                Set("CountQualifiedTeam", ref _countQualifiedTeam, null);
                return;

            }

            ushort countTeamFished, qualification, qualified;
            if (countQualificationGroup == null)
            {
                qualified = countQualifiedTeam.Value;
                QualificationStepSetting.ComputeGroup(FirstEliminationStep, qualified, out qualification, out countTeamFished);
            }
            else if (countQualifiedTeam == null)
            {
                qualification = countQualificationGroup.Value;
                QualificationStepSetting.ComputeGroup(qualification, FirstEliminationStep, out qualified,
                    out countTeamFished);
            }
            else return;

            Set("CountQualificationGroup", ref _countQualificationGroup, qualification);
            Set("CountQualifiedTeam", ref _countQualifiedTeam, qualified);
            CountFishPlayer = countTeamFished;
            OnPropertyChanged(() => CountMinTeamRegister);
            OnPropertyChanged(() => CountMaxTeamRegister);
        }

        public MatchSettingVm EliminationSettingVm
        {
            get => _eliminationSettingVm;
            set => Set(ref _eliminationSettingVm, value);
        }

        public MatchSettingVm ConsolingEliminationSettingVm
        {
            get => _consolingEliminationSettingVm;
            set => Set(ref _consolingEliminationSettingVm, value);
        }

        public MatchSettingVm QualificationSettingVm
        {
            get => _qualificationSettingVm;
            set => Set(ref _qualificationSettingVm, value);
        }

        public bool WithRevenge
        {
            get => _withRevenge;
            set => Set(ref _withRevenge, value);
        }

        #endregion

        internal void UpdateContest()
        {
            CurrentContest.EliminationSetting.FirstStep = FirstEliminationStep;
            EliminationSettingVm.UpdateFromModel(CurrentContest.EliminationSetting.MatchSetting);

            if (HasQualificationStep)
            {
                if (CountQualificationGroup == null)
                    throw new InvalidProgramException("NumberOfQualificationGroup can not be null at this step.");
                if (CountQualifiedTeam == null)
                    throw new InvalidProgramException("NumberOfQualifiedTeam can not be null at this step.");

                if (CurrentContest.QualificationSetting == null)
                {
                    CurrentContest.QualificationSetting =
                        new QualificationStepSetting(QualificationSettingVm.ToMatchSetting(),
                            CountQualificationGroup.Value,
                            FirstEliminationStep,
                            WithRevenge);
                }
                else
                {
                    QualificationSettingVm.UpdateFromModel(CurrentContest.QualificationSetting.MatchSetting);
                    CurrentContest.QualificationSetting.MatchWithRevenche = WithRevenge;
                    CurrentContest.QualificationSetting.SetCountGroup(CountQualificationGroup.Value,
                        FirstEliminationStep);
                }

                if (HasConsolingStep)
                {
                    if (FirstConsolingEliminationStep == null)
                        throw new Exception("La profondeur des phase de qualification doit être renseigné.");
                    if (CurrentContest.ConsolingEliminationSetting == null)
                    {
                        CurrentContest.ConsolingEliminationSetting =
                            new EliminationStepSetting(ConsolingEliminationSettingVm.ToMatchSetting(),
                                FirstConsolingEliminationStep.Value);
                    }
                    else
                    {
                        CurrentContest.ConsolingEliminationSetting.FirstStep = FirstConsolingEliminationStep.Value;
                        ConsolingEliminationSettingVm.UpdateFromModel(
                            CurrentContest.ConsolingEliminationSetting.MatchSetting);
                    }
                }
                else CurrentContest.ConsolingEliminationSetting = null;
            }
            else CurrentContest.QualificationSetting = null;

            var contestService = new ContestService();
            contestService.UpdateContest(CurrentContest);

        }
    }
}
