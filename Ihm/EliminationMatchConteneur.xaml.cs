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
            if (DataContext is EliminationMatchConteneurVm vm) vm.OnSizeChanged(e);
        }
    }
}
