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
        private bool _createBackup = true;
        private bool _createRestorePoint = true;

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
            _createBackup = createBackup;
            _createRestorePoint = createRestorePoint;
        }

        private void Log(string message)
        {
            TxtLog.Text += message + Environment.NewLine;
            Debug.WriteLine(message);
            TxtLog.ScrollToEnd();
        }

        private async void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            BtnInstall.IsEnabled = false;
            BtnBack.IsEnabled = false;
            BtnCancel.IsEnabled = false;
            TxtStatus.Text = "Starting installation...";
            TxtLog.Text = string.Empty;
            ProgressBar.Value = 0;
            
            try
            {
                await Task.Delay(100);
                
                // Step 1: Create backup
                ProgressBar.Value = 10;
                Log("Step 1/6: Creating system backup...");
                if (_createBackup)
                {
                    if (_backupManager.CreateBackup())
                    {
                        Log("  Success: System backup created");
                    }
                    else
                    {
                        Log("  Warning: Failed to create backup");
                    }
                }
                
                // Step 2: Create restore point
                ProgressBar.Value = 20;
                Log("Step 2/6: Creating Windows restore point...");
                if (_createRestorePoint)
                {
                    if (_backupManager.CreateRestorePoint(_selectedMode))
                    {
                        Log("  Success: Restore point created");
                    }
                    else
                    {
                        Log("  Warning: Failed to create restore point");
                    }
                }
                
                // Step 3: Apply registry tweaks
                ProgressBar.Value = 35;
                Log("Step 3/6: Applying registry tweaks...");
                if (_selectedMode == "CarShell")
                {
                    if (_registryManager.ApplyCarShellTweaks())
                    {
                        Log("  Success: CarShell registry tweaks applied");
                    }
                    else
                    {
                        Log("  Warning: Failed to apply CarShell tweaks");
                    }
                }
                else if (_selectedMode == "Kiosk")
                {
                    if (_registryManager.ApplyKioskTweaks())
                    {
                        Log("  Success: Kiosk registry tweaks applied");
                    }
                    else
                    {
                        Log("  Warning: Failed to apply Kiosk tweaks");
                    }
                }
                
                // Step 4: Configure services
                ProgressBar.Value = 50;
                Log("Step 4/6: Configuring services...");
                if (_selectedMode == "CarShell" || _selectedMode == "Kiosk")
                {
                    if (_serviceManager.DisableUnnecessaryServices())
                    {
                        Log("  Success: Unnecessary services disabled");
                    }
                    else
                    {
                        Log("  Warning: Failed to disable services");
                    }
                }
                
                // Step 5: Configure power settings
                ProgressBar.Value = 65;
                Log("Step 5/6: Configuring power settings...");
                if (_selectedMode == "CarShell")
                {
                    if (_powerManager.OptimizeForCar())
                    {
                        Log("  Success: Car power settings applied");
                    }
                    else
                    {
                        Log("  Warning: Failed to apply power settings");
                    }
                }
                else if (_selectedMode == "Kiosk")
                {
                    if (_powerManager.OptimizeForKiosk())
                    {
                        Log("  Success: Kiosk power settings applied");
                    }
                    else
                    {
                        Log("  Warning: Failed to apply power settings");
                    }
                }
                
                // Step 6: Configure shell
                ProgressBar.Value = 80;
                Log("Step 6/6: Configuring shell...");
                if (_selectedMode == "CarShell")
                {
                    string carShellPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "CarShell", "CarShell.exe");
                    if (File.Exists(carShellPath))
                    {
                        if (_shellManager.ConfigureForCarShell(carShellPath))
                        {
                            Log("  Success: Shell configured for CarShell");
                        }
                        else
                        {
                            Log("  Warning: Failed to configure shell");
                        }
                    }
                    else
                    {
                        Log("  Note: CarShell.exe not found. Shell not changed.");
                    }
                }
                else if (_selectedMode == "Kiosk")
                {
                    if (_kioskManager.ConfigureKioskMode())
                    {
                        Log("  Success: Kiosk mode configured");
                    }
                    else
                    {
                        Log("  Warning: Failed to configure kiosk mode");
                    }
                }
                
                ProgressBar.Value = 100;
                TxtStatus.Text = "Installation completed!";
                Log(string.Empty);
                Log("=== Installation Complete ===");
                Log("Mode: " + _selectedMode);
                Log("Profile: " + _selectedProfile);
                Log(string.Empty);
                Log("Note: Some changes require a system restart to take effect.");
                
                var finishPage = new FinishPage(_mainWindow);
                finishPage.SetResult(true, _selectedMode, _selectedProfile);
                _mainWindow.NavigateToFinish();
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "Installation failed!";
                Log(string.Empty);
                Log("=== ERROR ===");
                Log("Exception: " + ex.Message);
                Log(string.Empty);
                Log("Stack Trace:");
                Log(ex.StackTrace);
                
                BtnInstall.IsEnabled = true;
                BtnBack.IsEnabled = true;
                BtnCancel.IsEnabled = true;
            }
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