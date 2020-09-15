using System.Collections.Generic;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Players;
using Contest.Service;

namespace Contest.Ihm
{
    public class RegisterTeamVm : ViewModel
    {
        private readonly IContest _contest;
        private readonly IList<IPerson> _selectedPlayerList;

        #region Fields

        private string _name;

        #endregion

        #region Constructor
        
        public RegisterTeamVm(IContest contest, IList<IPerson> selectedPlayerList)
        {
            _contest = contest;
            _selectedPlayerList = selectedPlayerList;
            FlippingContainer.Instance.ComposeParts(this);
        }

        #endregion

        #region Properties

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        #endregion

        #region Commands

        public RelayCommand RegisterTeam => new RelayCommand(
            delegate
            {
                var service = new ContestService();
                service.CreateTeam(_contest, Name, _selectedPlayerList);
            },
            delegate
            {
                return !string.IsNullOrEmpty(Name);
            });

        #endregion
    }
}
