using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HaloRecordedAnimationTools.Helpers;
using HaloRecordedAnimationTools.IO;
using HaloRecordedAnimationTools.Blam;
using System.IO;
using System.ComponentModel;

namespace HaloRecordedAnimationTools
{
    // I should probably adhere to MVVM or whatever but I'm at my motivation's mercy.
    //
    // I prefer finishing something and getting it working before going back and making 
    // it pretty so I'm planning on refactoring this whole thing but I'll probably get it 
    // working and then end up not touching it unless something's broken because ^.
    public partial class MainWindow : Window
    {
        private SettingsWindow settingsWindow;
        private bool attached = false;
        private DispatcherTimer updateTimer;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly DispatcherTimer keyListener;
#pragma warning restore IDE0052 // Remove unread private members
        private const string scenarioFilter =
            "Halo 1 Scenario (*.scenario)|*.scenario|" +
            "All files (*.*)|*.*";

        public MainWindow()
        {
            InitializeComponent();
            keyListener = new DispatcherTimer(TimeSpan.FromMilliseconds(5), DispatcherPriority.Normal, CheckKeyDown, Dispatcher);
        }

        private void CheckKeyDown(object sender, EventArgs e)
        {
            if (!MiscExtensions.ApplicationHasFocus())
                if (KeyHelper.IsKeyDown(KeyInterop.KeyFromVirtualKey(Properties.Settings.Default.recordKey)))
                    ToggleRecording();
        }

        /*private void ToggleHook()
        {
            if (!attached)
            {
                if (Memory.Attach("haloce"))
                {
                    attached = true;
                    if (!int.TryParse(tbTickrate.Text, out tickRate))
                        tickRate = 30;
                    updateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(1000 / tickRate), DispatcherPriority.DataBind, UpdateValues, Dispatcher);
                    updateTimer.Start();
                }
                else
                    MessageBox.Show("haloce.exe not found.");
            }
            else
            {
                if (Memory.Detach())
                {
                    attached = false;
                    updateTimer.Stop();
                    updateTimer = null;
                    MessageBox.Show("Detached!");
                }
                else
                    MessageBox.Show("Couldn't detach!");
            }
        }*/

        private void ToggleRecording()
        {
            if (attached)
            {
                MessageBox.Show("Toggled! not really but this is a test y'know");
            }
            else
                MessageBox.Show("The haloce.exe process has not been attached yet!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /*private void UpdateValues(object sender, EventArgs e)
        {
            PlayerAnimInfo currentAnimInfo = PlayerAnimInfo.FromMemory();
            tbCrouch.Text = currentAnimInfo.Crouch.ToString();
            tbJump.Text = currentAnimInfo.Jump.ToString();
            tbFlashlight.Text = currentAnimInfo.Flashlight.ToString();
            tbAction.Text = currentAnimInfo.Action.ToString();
            tbMelee.Text = currentAnimInfo.Melee.ToString();
            tbReload.Text = currentAnimInfo.Reload.ToString();
            tbAttack.Text = currentAnimInfo.Attack.ToString();
            tbGrenade.Text = currentAnimInfo.Grenade.ToString();
            tbActSwap.Text = currentAnimInfo.ActionSwap.ToString();

            tbForward.Text = currentAnimInfo.playerForward.ToString();
            tbLeft.Text = currentAnimInfo.playerLeft.ToString();
            tbWeaponSlot.Text = currentAnimInfo.playerWeaponSlot.ToString();
            tbAimVector.Text = currentAnimInfo.playerAimVector.ToString();
            tbPosition.Text = currentAnimInfo.playerPosition.ToString();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleHook();
        }

        private void TbHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            toggleRecording = tbHotkey.Key;
        }

        private void BtnOpenScenario_Click(object sender, RoutedEventArgs e)
        {
            var openScenario = new OpenFileDialog()
            {
                Filter = "Scenario (*.scenario)|*.scenario"
            };
            if (openScenario.ShowDialog() == true)
            {
                Scenario scnr;
                using (EndianReader r = new EndianReader(openScenario.OpenFile(), EndianReader.Endian.Big))
                {
                    scnr = new Scenario(r);
                }
                lbTestReadAnimData.ItemsSource = scnr.RecordedAnimations;
            }
        }*/

        private void OpenScenario(string filePath)
        {
            Scenario scnr;
            using (EndianReader r = new EndianReader(File.Open(filePath, FileMode.Open), Endian.Big))
            {
                scnr = new Scenario(filePath, r);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainGrid.Focus();
        }

        private void MiOpen_Click(object sender, RoutedEventArgs e)
        {
            var openScenario = new OpenFileDialog()
            {
                Filter = scenarioFilter 
            };

            if (openScenario.ShowDialog() == true)
            {
                OpenScenario(openScenario.FileName);
            }
        }

        private void MiExit_Click(object sender, RoutedEventArgs e) => Close();

        private void MiSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settingsWindow == null || !settingsWindow.IsLoaded)
                settingsWindow = new SettingsWindow();

            settingsWindow.Show();
            if (settingsWindow.WindowState == WindowState.Minimized)
                settingsWindow.WindowState = WindowState.Normal;
            settingsWindow.Focus();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            base.OnClosing(e);
            settingsWindow?.Close();
            Application.Current.Shutdown();
        }
    }
}
