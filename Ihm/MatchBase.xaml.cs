using System.Windows;
using System.Windows.Controls;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for MatchViewer.xaml
    /// </summary>
    public partial class MatchBase
    {
        public MatchBase()
        {
            InitializeComponent();
        }

        private void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
