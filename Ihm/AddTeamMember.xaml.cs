using System.Collections.Generic;
using Contest.Domain.Players;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for AddTeamMember.xaml
    /// </summary>
    public partial class AddTeamMember
    {
        public AddTeamMember(IList<ITeam> selectableTeam)
        {
            InitializeComponent();
            var viewModel = new AddTeamMemberVm(selectableTeam);
            DataContext = viewModel;
            viewModel.RequestClose += o => Close();
        }
    }
}
