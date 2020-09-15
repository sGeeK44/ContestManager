using System.Windows.Input;
using Contest.Domain.Games;
using Microsoft.Xaml.Behaviors.Core;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for RegisterTeam.xaml
    /// </summary>
    public partial class RegisterTeam
    {
        public RegisterTeam()
        {
            InitializeComponent();
        }

        public ICommand CloseCommand => new ActionCommand(Close);
    }
}
