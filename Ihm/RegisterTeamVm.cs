using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class RegisterTeamVm : ViewModel
    {
        #region Fields

        private string _name;

        #endregion

        #region MEF Import

        [Import]
        private ITeamFactory TeamFactory { get; set; }

        #endregion

        #region Constructor
        
        public RegisterTeamVm(IContest currentContest)
        {
            FlippingContainer.Instance.ComposeParts(this);
            RegisterTeam = new RelayCommand(
                delegate
                {
                    CloseCommand.Execute(TeamFactory.Create(currentContest, Name));
                },
                delegate
                {
                    return !string.IsNullOrEmpty(Name);
                });
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        #endregion

        #region Commands

        public RelayCommand RegisterTeam { get; set; }

        #endregion
    }
}
