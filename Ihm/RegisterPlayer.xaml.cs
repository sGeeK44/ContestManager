using Contest.Domain.Players;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for RegisterPlayer.xaml
    /// </summary>
    public partial class RegisterPlayer
    {
        public RegisterPlayer(Person personToUpdate = null)
        {
            InitializeComponent();
            var viewModel = new RegisterPlayerVm(personToUpdate);
            DataContext = viewModel;
            viewModel.RequestClose += o => Close();
        }
    }
}
