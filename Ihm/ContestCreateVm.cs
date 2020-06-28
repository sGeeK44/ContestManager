using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Settings;

namespace Contest.Ihm
{
    public class ContestCreateVm : ViewModel
    {
        #region Dependencies

        [Import]
        private IAddressFactory AddressFactory { get; set; }

        [Import]
        private IGameSettingFactory GameSettingFactory { get; set; }

        #endregion

        #region private field
        
        private uint _countMaxPlayerByTeam;
        private uint _countMinPlayerByTeam;
        private bool _indoor;

        #endregion

        #region Constructors

        public ContestCreateVm()
        {
            // Do MEF resolution to inject dependencies.
            FlippingContainer.Instance.ComposeParts(this);

            //Default values.
            CountMinPlayerByTeam = 1;
            CountMaxPlayerByTeam = 3;
            CountField = 2;
            Date = DateTime.Today;

            Create = new RelayCommand(
                delegate
                    {
                        var newContest = Domain.Games.Contest.Create(Date, PhysicalSetting.Create(AddressFactory.Create(0, Street, ZipCode, City),
                                                                                              Indoor ? AreaType.Indoor : AreaType.Outdoor,
                                                                                              CountField),
                                                                 GameSettingFactory.Create(CountMinPlayerByTeam, CountMaxPlayerByTeam));
                        CloseCommand.Execute(newContest);
                    },
                delegate
                    {
                        return true;
                    });
        }

        #endregion

        #region Properties VM

        public uint CountMaxPlayerByTeam
        {
            get => _countMaxPlayerByTeam;
            set => Set(ref _countMaxPlayerByTeam, value);
        }

        public uint CountMinPlayerByTeam
        {
            get => _countMinPlayerByTeam;
            set => Set(ref _countMinPlayerByTeam, value);
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public bool Indoor
        {
            get => _indoor;
            set
            {
                Set(ref _indoor, value);
                OnPropertyChanged(() => Outdoor);
            }
        }

        public object Outdoor => !Indoor;

        public ushort CountField { get; set; }
        public DateTime Date { get; set; }
        public RelayCommand Create { get; private set; }

        #endregion
    }
}
