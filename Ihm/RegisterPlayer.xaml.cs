using System.Windows;
using System.Windows.Input;
using Contest.Domain.Players;
using Microsoft.Xaml.Behaviors.Core;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for RegisterPlayer.xaml
    /// </summary>
    public partial class RegisterPlayer
    {
        public RegisterPlayer()
        {
            InitializeComponent();
        }

        public ICommand CloseCommand => new ActionCommand(Close);
    }
}
