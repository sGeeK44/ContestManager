using System;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Service;

namespace Contest.Ihm
{
    public class ContestCreateVm : ViewModel
    {
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
                    var command = new CreateContestCmd
                    {
                        Date = Date,
                        StreetNumber = 0,
                        Street = Street,
                        ZipCode = ZipCode,
                        City = City,
                        Indoor = Indoor,
                        CountField = CountField,
                        CountMinPlayerByTeam = CountMinPlayerByTeam,
                        CountMaxPlayerByTeam = CountMaxPlayerByTeam
                    };

                    var service = new ContestService();
                    var newContest = service.Create(command);
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
