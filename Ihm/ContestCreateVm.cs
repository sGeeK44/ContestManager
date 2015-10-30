using System;
using System.Collections.ObjectModel;
using Contest.Business;
using Contest.Core.Helper;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class ContestCreateVm : ViewModel
    {
        private GameSetting.TypeOfPuck _typeOfPuck;
        private GameSetting.TypeOfPlayGround _typeOfBoard;
        private Business.Contest.TypeOfGame _typeOfGame;
        private uint _countMaxPlayerByTeam;
        private uint _countMinPlayerByTeam;
        private bool _indoor;

        #region Constructors

        public ContestCreateVm()
        {
            //Load dropdown list
            AvailableTypeOfGame = new ObservableCollection<Business.Contest.TypeOfGame>(EnumHelper.GetValueList<Business.Contest.TypeOfGame>());
            AvailableTypeOfPuck = new ObservableCollection<GameSetting.TypeOfPuck>(EnumHelper.GetValueList<GameSetting.TypeOfPuck>());
            AvailableTypeOfBoard = new ObservableCollection<GameSetting.TypeOfPlayGround>(EnumHelper.GetValueList<GameSetting.TypeOfPlayGround>());

            //Default values.
            TypeOfPuck = GameSetting.TypeOfPuck.Fonte;
            CountMinPlayerByTeam = 1;
            CountMaxPlayerByTeam = 3;
            CountField = 2;
            Date = DateTime.Today;

            Create = new RelayCommand(
                delegate
                    {
                        var newContest = Business.Contest.Create(Date, PhysicalSetting.Create(Address.Create(0, Street, ZipCode, City),
                                                                                              Indoor ? AreaType.Indoor : AreaType.Outdoor,
                                                                                              CountField),
                                                                 GameSetting.Create(TypeOfBoard, TypeOfPuck, CountMinPlayerByTeam, CountMaxPlayerByTeam));
                        CloseCommand.Execute(newContest);
                    },
                delegate
                    {
                        return true;
                    });
        }

        #endregion

        #region Properties VM

        public Business.Contest.TypeOfGame TypeOfGame
        {
            get { return _typeOfGame; }
            set { Set(ref _typeOfGame, value); }
        }

        public ObservableCollection<Business.Contest.TypeOfGame> AvailableTypeOfGame { get; set; }

        public GameSetting.TypeOfPuck TypeOfPuck
        {
            get { return _typeOfPuck; }
            set
            {
                Set(ref _typeOfPuck, value);
                OnPropertyChanged(() => Distance);
            }
        }

        public ObservableCollection<GameSetting.TypeOfPuck> AvailableTypeOfPuck { get; set; }

        public GameSetting.TypeOfPlayGround TypeOfBoard
        {
            get { return _typeOfBoard; }
            set
            {
                Set(ref _typeOfBoard, value);
                OnPropertyChanged(()=>Distance);
            }
        }

        public ObservableCollection<GameSetting.TypeOfPlayGround> AvailableTypeOfBoard { get; set; }

        public double? Distance { get { return GameSetting.GetDistance(TypeOfBoard, TypeOfPuck); } }

        public uint CountMaxPlayerByTeam
        {
            get { return _countMaxPlayerByTeam; }
            set { Set(ref _countMaxPlayerByTeam, value); }
        }

        public uint CountMinPlayerByTeam
        {
            get { return _countMinPlayerByTeam; }
            set { Set(ref _countMinPlayerByTeam, value); }
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public bool Indoor
        {
            get { return _indoor; }
            set
            {
                Set(ref _indoor, value);
                OnPropertyChanged(() => Outdoor);
            }
        }

        public object Outdoor
        {
            get { return !Indoor; }
        }

        public ushort CountField { get; set; }
        public DateTime Date { get; set; }
        public RelayCommand Create { get; private set; }

        #endregion
    }
}
