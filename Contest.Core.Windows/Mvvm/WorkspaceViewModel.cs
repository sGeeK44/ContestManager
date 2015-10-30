using System;
using System.Windows.Input;
using Contest.Core.Windows.Commands;

namespace Contest.Core.Windows.Mvvm
{
    public class WorkspaceViewModel : ViewModel
    {
        public WorkspaceViewModel()
        {
            Close = new CloseCommand();
        }

        public ICommand Close { get; set; }

        public void OnRequestClose()
        {
            RequestClose();
        }

        public event Action RequestClose;
    }
}
