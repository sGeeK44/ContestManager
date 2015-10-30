using System;
using System.Collections.ObjectModel;
using System.Linq;
using Contest.Business;
using Contest.Core.Helper;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class ContestViewVm : ViewModel
    {
        public const string NOT_A_NUMBER = "N/A";
        public const string INFINIT = "++";
        private EliminationType _firstEliminationStep;
        private EliminationType? _firstConsolingEliminationStep;
        private bool _hasQualificationStep;
        private bool _hasConsolingStep;
        private ushort? _countQualificationGroup;
        private ushort? _countQualifiedTeam;
        private int? _countFishPlayer;
        private bool _withRevenge;
        private MatchSettingVm _eliminationSettingVM;
        private MatchSettingVm _consolingEliminationSettingVM;
        private MatchSettingVm _qualificationSettingVM;
        private IContest _currentContest;

        #region Constructors

        public ContestViewVm(IContest contest)
        {
            if (contest == null) throw new ArgumentNullException("contest");
            if (contest.EliminationSetting == null) throw new InvalidProgramException("Elimination setting can not be null.");
            
            FirstEliminationStep = contest.EliminationSetting.FirstStep;
            EliminationSettingVM = new MatchSettingVm(contest.EliminationSetting.MatchSetting);
            HasQualificationStep = contest.WithQualificationPhase;
            HasConsolingStep = contest.WithConsolante;

            if (contest.QualificationSetting != null)
            {
                CountQualificationGroup = contest.QualificationSetting.CountGroup;
                WithRevenge = contest.QualificationSetting.MatchWithRevenche;
                
                QualificationSettingVM = new MatchSettingVm(contest.QualificationSetting.MatchSetting);
            }
            else QualificationSettingVM = new MatchSettingVm();

            if (contest.ConsolingEliminationSetting != null)
            {
                FirstConsolingEliminationStep = contest.ConsolingEliminationSetting.FirstStep;
                ConsolingEliminationSettingVM = new MatchSettingVm(contest.ConsolingEliminationSetting.MatchSetting);
            }
            else ConsolingEliminationSettingVM = new MatchSettingVm();

            CurrentContest = contest;
        }

        public IContest CurrentContest
        {
            get { return _currentContest; }
            set { Set( ref _currentContest, value); }
        }

        #endregion

        #region Properties VM

        public string CountMinTeamRegister
        {
            get
            {
                var minTeam = Business.Contest.MinTeamRegister(CountTeamFirstEliminationStep, HasQualificationStep,
                                                                CountQualificationGroup, HasConsolingStep, CountTeamFirstConsolingEliminationStep);
                return minTeam != null ? minTeam.ToString() : NOT_A_NUMBER;
            }
        }

        public string CountMaxTeamRegister
        {
            get
            {
                var maxTeam = Business.Contest.MaxTeamRegister(CountTeamFirstEliminationStep, HasQualificationStep, CountQualificationGroup, HasConsolingStep, CountTeamFirstConsolingEliminationStep);
                if (maxTeam == null) return NOT_A_NUMBER;
                return maxTeam == int.MaxValue ? INFINIT : maxTeam.ToString();
            }
        }

        public uint CountMaxPlayerByTeam
        {
            get { return CurrentContest.MaximumPlayerByTeam; }
            set
            {
                CurrentContest.MaximumPlayerByTeam = value;
                OnPropertyChanged(()=>CountMaxPlayerByTeam);
            }
        }

        public uint CountMinPlayerByTeam
        {
            get { return CurrentContest.MinimumPlayerByTeam; }
            set
            {
                CurrentContest.MinimumPlayerByTeam = value;
                OnPropertyChanged(() => CountMinPlayerByTeam);
            }
        }

        public ushort CountField
        {
            get { return CurrentContest.CountField; }
            set
            {
                CurrentContest.CountField = value;
                OnPropertyChanged(()=>CountField);
            }
        }

        public ObservableCollection<EliminationType> AvailableEliminationStep
        {
            get
            {
                return new ObservableCollection<EliminationType>(EnumHelper.GetValueList<EliminationType>());
            }
        }

        public EliminationType FirstEliminationStep
        {
            get { return _firstEliminationStep; }
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

        public ushort CountTeamFirstEliminationStep
        {
            get { return (ushort)((ushort)FirstEliminationStep * 2); }
        }

        public bool HasQualificationStep
        {
            get { return _hasQualificationStep; }
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
            get { return _hasConsolingStep; }
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
            get { return _firstConsolingEliminationStep; }
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

        public ObservableCollection<EliminationType> AvailableConsolingEliminationStep
        {
            get
            {
                return new ObservableCollection<EliminationType>(EnumHelper.GetValueList<EliminationType>());
            }
        }

        public ushort? CountTeamFirstConsolingEliminationStep
        {
            get { return FirstConsolingEliminationStep != null
                       ? (ushort?)((ushort)FirstConsolingEliminationStep.Value * 2)
                       : null; }
        }

        public int? CountFishPlayer
        {
            get { return _countFishPlayer; }
            set
            {
                Set(ref _countFishPlayer, value);
                OnPropertyChanged(()=>HasFishPlayer);
            }
        }

        public bool HasFishPlayer { get { return CountFishPlayer != null && CountFishPlayer != 0; } }

        public ushort? CountQualifiedTeam
        {
            get { return _countQualifiedTeam; }
            set
            {
                SetQualificationCount(null, value);
            }
        }

        public ushort? CountQualificationGroup
        {
            get { return _countQualificationGroup; }
            set { SetQualificationCount(value, null); }
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

        public MatchSettingVm EliminationSettingVM
        {
            get { return _eliminationSettingVM; }
            set { Set(ref _eliminationSettingVM, value); }
        }

        public MatchSettingVm ConsolingEliminationSettingVM
        {
            get { return _consolingEliminationSettingVM; }
            set { Set(ref _consolingEliminationSettingVM, value); }
        }

        public MatchSettingVm QualificationSettingVM
        {
            get { return _qualificationSettingVM; }
            set { Set(ref _qualificationSettingVM, value); }
        }

        public bool WithRevenge
        {
            get { return _withRevenge; }
            set { Set(ref _withRevenge, value); }
        }

        #endregion

        internal void UpdateContest()
        {
            CurrentContest.EliminationSetting.FirstStep = FirstEliminationStep;
            EliminationSettingVM.UpdateFromModel(CurrentContest.EliminationSetting.MatchSetting);

            if (HasQualificationStep)
            {
                if (CountQualificationGroup == null)
                    throw new InvalidProgramException("NumberOfQualificationGroup can not be null at this step.");
                if (CountQualifiedTeam == null)
                    throw new InvalidProgramException("NumberOfQualifiedTeam can not be null at this step.");

                if (CurrentContest.QualificationSetting == null)
                {
                    CurrentContest.QualificationSetting =
                        new QualificationStepSetting(QualificationSettingVM.ToMatchSetting(),
                            CountQualificationGroup.Value,
                            FirstEliminationStep,
                            WithRevenge);
                }
                else
                {
                    QualificationSettingVM.UpdateFromModel(CurrentContest.QualificationSetting.MatchSetting);
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
                            new EliminationStepSetting(ConsolingEliminationSettingVM.ToMatchSetting(),
                                FirstConsolingEliminationStep.Value);
                    }
                    else
                    {
                        CurrentContest.ConsolingEliminationSetting.FirstStep = FirstConsolingEliminationStep.Value;
                        ConsolingEliminationSettingVM.UpdateFromModel(
                            CurrentContest.ConsolingEliminationSetting.MatchSetting);
                    }
                }
                else CurrentContest.ConsolingEliminationSetting = null;
            }
            else CurrentContest.QualificationSetting = null;
        }
    }
}
