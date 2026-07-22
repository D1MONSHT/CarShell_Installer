# CarShellInstaller

WPF .NET 8 application for installing and configuring CarShell with different Windows modes.

## Features

### Current Release (v1.0)
- WPF .NET 8 application
- Installation wizard with multiple pages
- Mode selection: Windows / CarShell / Kiosk
- JSON profile system
- Settings backup and restore
- Core management modules:
  - Shell management
  - Service control
  - Power management
  - Registry operations
  - System restoration

### Planned Features
- CarShell installation
- Maps integration
- WebView2 support
- CAN driver configuration
- Bluetooth setup
- Auto-updates via Updater

## Project Structure

CarShellInstaller/
  .github/workflows/
    build.yml          # Build on push to main
    release.yml        # Create release on tag push
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
  BUILD.md
  README.md
  LICENSE
  .gitignore

## Getting Started

### Option 1: Download Release (Recommended)
1. Go to [Releases](https://github.com/D1MONSHT/CarShell_Installer/releases)
2. Download the latest CarShellInstaller.exe
3. Run as Administrator

### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com/D1MONSHT/CarShell_Installer.git
   cd CarShell_Installer
   ```

2. Install .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0

3. Build and run:
   ```bash
   dotnet restore CarShellInstaller.sln
   dotnet build CarShellInstaller.sln --configuration Release
   dotnet run --project CarShellInstaller/CarShellInstaller.csproj
   ```

Or publish for distribution:
   ```bash
   dotnet publish CarShellInstaller/CarShellInstaller.csproj --configuration Release --output ./publish --self-contained true
   ./publish/CarShellInstaller.exe
   ```

## Profiles

- **Standard.json** - Default Windows configuration with minimal changes
- **CarMode.json** - Optimized configuration for CarShell in automotive environment
- **ExtremeKiosk.json** - Full kiosk mode with maximum security and lockdown

## GitHub Actions

### Automatic Builds
- Builds on every push to main branch
- Publishes artifacts for download

### Creating a Release
To create a new release, push a tag:
```bash
git tag v1.1
git push origin v1.1
```

The release workflow will automatically:
- Build the project
- Create a GitHub release
- Upload the published files

## Requirements

- .NET 8 SDK (for building from source)
- Windows 10/11
- Administrator privileges (for running)

## License

MIT License - see [LICENSE](LICENSE) file for details.

## Repository

https://github.com/D1MONSHT/CarShell_Installer