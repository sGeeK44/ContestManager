using System;
using System.Windows.Input;
using Contest.Core.Windows.Commands;

namespace Contest.Core.Windows.Mvvm
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            CloseCommand = new RelayCommand(OnRequestClose);
        }

        public ICommand CloseCommand;

        public event Action<object> RequestClose;

        protected virtual void OnRequestClose(object obj)
        {
            var handler = RequestClose;
            if (handler != null) handler(obj);
        }
    }
}
