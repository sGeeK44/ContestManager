using System.Windows;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for MatchViewer.xaml
    /// </summary>
    public partial class EliminationMatchConteneur
    {
        public EliminationMatchConteneur()
        {
            InitializeComponent();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vm = DataContext as EliminationMatchConteneurVm;
            if (vm != null) vm.OnSizeChanged(e);
        }
    }
}
