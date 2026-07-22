using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CarShellInstaller.Core;

namespace CarShellInstaller.Views
{
    public partial class InstallPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly BackupManager _backupManager;
        private readonly ShellManager _shellManager;
        private readonly ServiceManager _serviceManager;
        private readonly PowerManager _powerManager;
        private readonly RegistryManager _registryManager;
        private readonly KioskManager _kioskManager;
        private string _selectedMode = "Windows";
        private string _selectedProfile = "Standard";

        public InstallPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _backupManager = new BackupManager();
            _shellManager = new ShellManager();
            _serviceManager = new ServiceManager();
            _powerManager = new PowerManager();
            _registryManager = new RegistryManager();
            _kioskManager = new KioskManager();
        }

        public void SetOptions(string mode, string profile, bool createBackup, bool createRestorePoint)
        {
            _selectedMode = mode;
            _selectedProfile = profile;
        }

        private void Log(string message)
        {
            TxtLog.Text += message + Environment.NewLine;
            Debug.WriteLine(message);
        }

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            BtnInstall.IsEnabled = false;
            BtnBack.IsEnabled = false;
            BtnCancel.IsEnabled = false;
            TxtStatus.Text = "Starting installation...";
            TxtLog.Text = "";
            ProgressBar.Value = 0;
            await Task.Delay(100);
            Log("Starting installation...");
            ProgressBar.Value = 10;
            Log("Applying configuration...");
            ProgressBar.Value = 60;
            Log("Loading profile...");
            ProgressBar.Value = 80;
            Log("Finalizing installation...");
            ProgressBar.Value = 100;
            TxtStatus.Text = "Installation completed!";
            var finishPage = new FinishPage(_mainWindow);
            finishPage.SetResult(true, _selectedMode, _selectedProfile);
            _mainWindow.NavigateToFinish();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToOptions();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}