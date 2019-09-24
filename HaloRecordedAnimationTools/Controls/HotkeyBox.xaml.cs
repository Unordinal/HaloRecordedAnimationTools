using System;
using System.Windows.Controls;
using System.Windows.Input;
using HaloRecordedAnimationTools.Helpers;

namespace HaloRecordedAnimationTools.Controls
{
    /// <summary>
    /// Interaction logic for HotkeyBox.xaml
    /// </summary>
    public partial class HotkeyBox : UserControl
    {
        private Key key = Key.None;
        public Key Key
        {
            get => key;
            set
            {
                HotkeyTextBox.Text = value.ToString();
                key = value;
                if (Keyboard.PrimaryDevice?.ActiveSource != null)
                {
                    var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, Environment.TickCount, value)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    OnHotkeyChanged(keyEventArgs);
                }
            }
        }

        public HotkeyBox()
        {
            InitializeComponent();
            HotkeyTextBox.SetupFocusSelect();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key = e.Key;
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(HotkeyTextBox), null);
            Keyboard.ClearFocus();
        }

        protected virtual void OnHotkeyChanged(KeyEventArgs e) =>
            HotkeyChanged?.Invoke(this, e);

        public event KeyEventHandler HotkeyChanged;
    }
}
