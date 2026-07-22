using System.Windows;
using System.Windows.Controls;

namespace CarShellInstaller.Views
{
    public partial class FinishPage : Page
    {
        private readonly MainWindow _mainWindow;

        public FinishPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        public void SetResult(bool success, string mode, string profile)
        {
            if (success)
            {
                TxtHeader.Text = "Installation Complete!";
                TxtSummary.Text = "CarShell Installer has been successfully installed and configured.";
                TxtMode.Text = "Mode: " + mode;
                TxtProfile.Text = "Profile: " + profile;
                TxtBackup.Text = "Backup: Created";
                TxtError.Visibility = Visibility.Collapsed;
            }
            else
            {
                TxtHeader.Text = "Installation Failed";
                TxtSummary.Text = "There was an error during installation.";
                TxtMode.Text = "Mode: " + mode;
                TxtProfile.Text = "Profile: " + profile;
                TxtBackup.Text = "Backup: May be incomplete";
                TxtError.Text = "Please check the installation log for details.";
                TxtError.Visibility = Visibility.Visible;
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}