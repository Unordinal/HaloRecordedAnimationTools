using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HaloRecordedAnimationTools
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Key RecordKey
        {
            get => KeyInterop.KeyFromVirtualKey(Properties.Settings.Default.recordKey);
            set
            {
                if (hbHotkey.Key != value)
                    hbHotkey.Key = value;
                Properties.Settings.Default.recordKey = KeyInterop.VirtualKeyFromKey(value);
                Properties.Settings.Default.Save();
            }
        }
        public SettingsWindow()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            hbHotkey.Key = RecordKey;
            hbHotkey.HotkeyChanged += HbHotkey_HotkeyChanged;
        }

        private void HbHotkey_HotkeyChanged(object sender, KeyEventArgs e) =>
            RecordKey = e.Key;
    }
}
