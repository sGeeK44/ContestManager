using System.Windows;
using Contest.Core.Component;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            FlippingContainer.Instance.Current = new DirectoryComposer();
            DataContext = new MainWindowVm();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Etes-vous sür de vouloir quitter l'application ?", "Contest", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel) e.Cancel = true;
            var viewModel = (MainWindowVm)DataContext;
            viewModel.Exit.Execute(null);
        }
    }
}
