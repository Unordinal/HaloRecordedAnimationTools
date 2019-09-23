using System;
using System.Windows;
using System.Windows.Controls;

namespace HaloRecordedAnimationTools.Helpers
{
    public static class TextBoxExtensions
    {
        public static void SetupFocusSelect(this TextBox tb)
        {
            tb.GotFocus += SelectAll;
            tb.LostFocus += Deselect;
            tb.PreviewMouseLeftButtonDown += SelectivelyIgnoreMouseButton;
        }

        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()));
        }

        private static void Deselect(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                tb.Select(0, 0);
        }

        private static void SelectivelyIgnoreMouseButton(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();
            }
        }
    }
}
