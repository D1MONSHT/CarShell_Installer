using System;
using System.Windows;
using System.Windows.Controls;
using CarShellInstaller.Views;

namespace CarShellInstaller
{
    public partial class MainWindow : Window
    {
        public string SelectedMode { get; set; } = "Windows";
        public string SelectedProfile { get; set; } = "Standard";
        public bool CreateBackup { get; set; } = true;
        public bool CreateRestorePoint { get; set; } = true;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WelcomePage(this));
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }

        public void NavigateToModeSelection()
        {
            MainFrame.Navigate(new ModePage(this));
        }

        public void NavigateToOptions()
        {
            MainFrame.Navigate(new OptionsPage(this));
        }

        public void NavigateToInstall()
        {
            var installPage = new InstallPage(this);
            installPage.SetOptions(
                SelectedMode,
                SelectedProfile,
                CreateBackup,
                CreateRestorePoint);

            MainFrame.Navigate(installPage);
        }

        public void NavigateToFinish()
        {
            MainFrame.Navigate(new FinishPage(this));
        }
    }
}