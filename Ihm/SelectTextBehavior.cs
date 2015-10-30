using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Contest.Ihm
{
    public class SelectTextBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AttachGotFocus;
            
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= AttachGotFocus;
        }

        public void AttachGotFocus(object sender, RoutedEventArgs args)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;
            if (textBox.Text != null && textBox.SelectionLength != textBox.Text.Length) textBox.SelectAll();
        }
    }
}
