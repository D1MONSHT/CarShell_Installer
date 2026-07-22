using System;
using System.Windows;
using System.Windows.Navigation;
using CarShellInstaller.Views;
using System.Windows.Controls;
namespace CarShellInstaller
{
    public partial class MainWindow : Window
    {
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
            MainFrame.Navigate(new InstallPage(this));
        }

        public void NavigateToFinish()
        {
            MainFrame.Navigate(new FinishPage(this));
        }
    }
}
