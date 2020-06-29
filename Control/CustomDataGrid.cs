﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Contest.Control
{
    public class CustomDataGrid : DataGrid
    {

        public CustomDataGrid()
        {
            SelectionChanged += CustomDataGrid_SelectionChanged;
            
        }

        void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }
        #region SelectedItemsList

        public IList SelectedItemsList
        {
            get => (IList)GetValue(SelectedItemsListProperty);
            set => SetValue(SelectedItemsListProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid), new PropertyMetadata(null));

        #endregion
    }
}
