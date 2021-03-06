﻿using System.Windows;

namespace Contest.Ihm
{
    /// <summary>
    /// Interaction logic for MatchViewer.xaml
    /// </summary>
    public partial class EliminationMatchViewer
    {
        public EliminationMatchViewer()
        {
            InitializeComponent();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is EliminationMatchViewerVm vm) vm.OnSizeChanged();
        }
    }
}
