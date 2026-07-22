using System.Windows;
using System.Windows.Controls;

namespace CarShellInstaller.Views
{
    public partial class OptionsPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly InstallPage _installPage;
        
        public string SelectedProfile { get; private set; } = "Standard";
        public bool CreateBackup { get; private set; } = true;
        public bool CreateRestorePoint { get; private set; } = true;
        public bool DisableServices { get; private set; } = true;
        public bool OptimizeWindows { get; private set; } = true;
        public bool ConfigurePower { get; private set; } = true;
        public bool ApplyRegistryTweaks { get; private set; } = true;

        public OptionsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            
            // Initialize default values from UI
            CmbProfile.SelectionChanged += (s, e) => 
            {
                if (CmbProfile.SelectedItem is ComboBoxItem item)
                {
                    SelectedProfile = item.Content.ToString();
                }
            };
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            // Get current selections
            SelectedProfile = (CmbProfile.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Standard";
            CreateBackup = ChkCreateBackup.IsChecked ?? false;
            CreateRestorePoint = ChkRestorePoint.IsChecked ?? false;
            DisableServices = ChkDisableServices.IsChecked ?? false;
            OptimizeWindows = ChkOptimizeWindows.IsChecked ?? false;
            ConfigurePower = ChkPowerManagement.IsChecked ?? false;
            ApplyRegistryTweaks = ChkRegistryTweaks.IsChecked ?? false;
            
            // Create install page with options
            var installPage = new InstallPage(_mainWindow);
            installPage.SetOptions(
                _mainWindow.SelectedMode, 
                SelectedProfile, 
                CreateBackup,
                CreateRestorePoint
            );
            _mainWindow.NavigateToInstall();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToModeSelection();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}