using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Business;
using Contest.Core.Windows.Commands;
using Contest.Core.Windows.Mvvm;

namespace Contest.Ihm
{
    public class AddTeamMemberVm : ViewModel
    {
        #region Constructor
        
        public AddTeamMemberVm(IList<ITeam> selectableTeam)
        {
            if (selectableTeam == null) throw new ArgumentNullException("selectableTeam");
            if (selectableTeam.Count == 0) throw new ArgumentException("Aucune équipe de disponible.", "selectableTeam");

            AvailableTeamList = selectableTeam;
            SelectedTeam = AvailableTeamList.First();
            Select = new RelayCommand(
                delegate
                {
                    CloseCommand.Execute(SelectedTeam);
                },
                delegate
                {
                    return true;
                });
        }

        #endregion

        #region Properties}

        public IList<ITeam> AvailableTeamList { get; set; }

        public ITeam SelectedTeam { get; set; }

        #endregion

        #region Commands

        public RelayCommand Select { get; set; }

        #endregion
    }
}
