using System.Collections.Generic;
using System.Linq;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Games;
using Contest.Domain.Players;
using Contest.Service;

namespace Contest.Ihm
{
    public class AddTeamMemberVm : ViewModel
    {
        #region Constructor
        
        public AddTeamMemberVm(IContest currentContest, IList<IPerson> selectedPlayers)
        {
            SelectedPlayers = selectedPlayers;
            AvailableTeamList = currentContest.TeamList.Where(_ => _.Members.Count < currentContest.MaximumPlayerByTeam).ToList();
            SelectedTeam = AvailableTeamList.FirstOrDefault();
        }

        #endregion

        #region Properties

        public IContest Contest { get; set; }

        public IList<ITeam> AvailableTeamList { get; set; }

        public ITeam SelectedTeam { get; set; }
        public IList<IPerson> SelectedPlayers { get; }

        #endregion

        #region Commands

        public RelayCommand Select => new RelayCommand(
            delegate
            {
                var service = new ContestService();
                service.AddPlayer(SelectedTeam, SelectedPlayers);
            },
            delegate
            {
                return true;
            });

        #endregion
    }
}
