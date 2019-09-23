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
        public Key Key { get; private set; }
        public HotkeyBox()
        {
            InitializeComponent();
            HotkeyTextBox.SetupFocusSelect();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key = e.Key;
            HotkeyTextBox.Text = e.Key.ToString();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(HotkeyTextBox), null);
            Keyboard.ClearFocus();
        }
    }
}
