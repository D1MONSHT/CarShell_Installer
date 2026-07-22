using System.Windows;
using System.Windows.Controls;

namespace CarShellInstaller.Views
{
    public partial class ModePage : Page
    {
        private readonly MainWindow _mainWindow;
        public string SelectedMode { get; private set; } = "Windows";

        public ModePage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            RbtnWindows.Checked += (s, e) => SelectedMode = "Windows";
            RbtnCarShell.Checked += (s, e) => SelectedMode = "CarShell";
            RbtnKiosk.Checked += (s, e) => SelectedMode = "Kiosk";
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateToOptions();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.NavigateTo(new WelcomePage(_mainWindow));
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