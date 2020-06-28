using Contest.Domain.Games;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for RegisterTeam.xaml
    /// </summary>
    public partial class RegisterTeam
    {
        public RegisterTeam(IContest contest)
        {
            InitializeComponent();
            var viewModel = new RegisterTeamVm(contest);
            DataContext = viewModel;
            viewModel.RequestClose += o => Close();
        }
    }
}
