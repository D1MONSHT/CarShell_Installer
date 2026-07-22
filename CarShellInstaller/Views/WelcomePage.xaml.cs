using System.Windows;
using System.Windows.Controls;

namespace CarShellInstaller.Views
{
    public partial class WelcomePage : Page
    {
        private readonly MainWindow _mainWindow;

        public WelcomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
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