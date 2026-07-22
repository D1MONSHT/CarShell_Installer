using System.Windows;
using System.Windows.Controls;

namespace CarShellInstaller.Views
{
    public partial class OptionsPage : Page
    {
        private readonly MainWindow _mainWindow;

        public OptionsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            
            CmbProfile.SelectionChanged += (s, e) =>
            {
                if (CmbProfile.SelectedItem is ComboBoxItem item)
                {
                    _mainWindow.SelectedProfile = item.Content.ToString();
                }
            };
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.SelectedProfile = (CmbProfile.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Standard";
            _mainWindow.CreateBackup = ChkCreateBackup.IsChecked ?? false;
            _mainWindow.CreateRestorePoint = ChkRestorePoint.IsChecked ?? false;
            
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