using System;
using System.Windows.Input;

namespace Contest.Core.Windows.Commands
{
    public class CloseCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
        }
    }
}
