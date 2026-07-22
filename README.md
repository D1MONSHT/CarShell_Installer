# CarShellInstaller

WPF .NET 8 application for installing and configuring CarShell with different Windows modes.

## Features

### Current Release (v1.0)
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
├── CarShellInstaller.sln
├── CarShellInstaller/
│   ├── CarShellInstaller.csproj
│   ├── app.manifest
│   ├── App.xaml
│   ├── MainWindow.xaml
│   ├── Views/          # UI pages
│   ├── Core/           # Management modules
│   ├── Profiles/       # JSON configuration profiles
│   ├── Scripts/        # PowerShell scripts
│   └── Resources/      # Styles and assets
├── README.md
├── LICENSE
└── .gitignore

## Getting Started

1. Clone the repository
2. Open CarShellInstaller.sln in Visual Studio 2022
3. Build the solution
4. Run the application (requires Administrator privileges)

## Profiles

- Standard.json - Default Windows configuration
- CarMode.json - CarShell optimized settings
- ExtremeKiosk.json - Full kiosk mode configuration

## Requirements

- .NET 8 SDK
- Windows 10/11
- Visual Studio 2022 (optional for development)

## License

MIT License - see LICENSE file for details.