using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for AddTeamMember.xaml
    /// </summary>
    public partial class AddTeamMember
    {
        public AddTeamMember()
        {
            InitializeComponent();
        }

        public ICommand CloseCommand => new ActionCommand(Close);
    }
}
