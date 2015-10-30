using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Component;
using Contest.Core.Repository;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class ContestSelectVm : ViewModel
    {
        #region MEF Import
        
        [Import]
        private IRepository<IContest> ContestRepository { get; set; } 

        #endregion

        #region Constructors

        public ContestSelectVm()
        {
            FlippingContainer.Instance.ComposeParts(this);
            ContestList = new ObservableCollection<IContest>(ContestRepository.Find(_ => true));
            Load = new RelayCommand(
                delegate
                {
                    CloseCommand.Execute(SelectedContest);
                },
                delegate { return true; });
        }

        #endregion

        #region Properties VM

        public RelayCommand Load { get; private set; }

        public IContest SelectedContest { get; set; }

        public ObservableCollection<IContest> ContestList { get; set; }

        #endregion
    }
}
