# CarShellInstaller

WPF .NET 8 application for installing and configuring CarShell with different Windows modes.

## Features

### Current Release v1.0
- WPF .NET 8 application
- Installation wizard with multiple pages
- Mode selection: Windows / CarShell / Kiosk
- JSON profile system
- Settings backup and restore
- Core management modules

### Planned Features
- CarShell installation
- Maps integration
- WebView2 support
- CAN driver configuration
- Bluetooth setup
- Auto-updates via Updater

## Project Structure
CarShellInstaller/
  CarShellInstaller.sln
  CarShellInstaller/
    CarShellInstaller.csproj
    app.manifest
    App.xaml
    App.xaml.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    Views/
      WelcomePage.xaml
      WelcomePage.xaml.cs
      ModePage.xaml
      ModePage.xaml.cs
      OptionsPage.xaml
      OptionsPage.xaml.cs
      InstallPage.xaml
      InstallPage.xaml.cs
      FinishPage.xaml
      FinishPage.xaml.cs
    Core/
      RegistryManager.cs
      ShellManager.cs
      ServiceManager.cs
      PowerManager.cs
      BackupManager.cs
      KioskManager.cs
    Profiles/
      Standard.json
      CarMode.json
      ExtremeKiosk.json
    Scripts/
      DisableServices.ps1
      EnableServices.ps1
      OptimizeWindows.ps1
      RestoreWindows.ps1
    Resources/
      Styles.xaml
      Images/
      Icons/
  README.md
  LICENSE
  .gitignore

## Getting Started
1. Clone: git clone https://github.com/D1MONSHT/CarShell_Installer.git
2. Open CarShellInstaller.sln in Visual Studio 2022
3. Build the solution
4. Run (requires Administrator privileges)

## Profiles
- Standard.json - Default Windows configuration
- CarMode.json - CarShell optimized settings
- ExtremeKiosk.json - Full kiosk mode

## Requirements
- .NET 8 SDK
- Windows 10/11
- Visual Studio 2022

## License
MIT License - see LICENSE file for details.

## Repository
https://github.com/D1MONSHT/CarShell_Installer