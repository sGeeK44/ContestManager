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
    }
}
