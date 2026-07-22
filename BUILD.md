# Build Instructions

## Prerequisites
- .NET 8 SDK (https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10/11

## Build with .NET CLI

### 1. Clone the repository
```bash
git clone https://github.com/D1MONSHT/CarShell_Installer.git
cd CarShell_Installer
```

### 2. Restore dependencies
```bash
dotnet restore CarShellInstaller.sln
```

### 3. Build the project
```bash
dotnet build CarShellInstaller.sln --configuration Release
```

### 4. Run the application
```bash
# Navigate to publish directory
cd CarShellInstaller\bin\Release\net8.0-windows\publish

# Run (requires Administrator privileges)
CarShellInstaller.exe
```

### 5. Publish for distribution
```bash
dotnet publish CarShellInstaller/CarShellInstaller.csproj \
  --configuration Release \
  --output ./publish \
  --self-contained true \
  -p:PublishSingleFile=false \
  -p:PublishTrimmed=false
```

The published files will be in the ./publish directory.

## GitHub Actions

The repository includes a GitHub Actions workflow that automatically:
- Builds the project on every push to main
- Publishes artifacts
- Creates a release when pushing tags

To create a release:
```bash
git tag v1.0
git push origin v1.0
```

## Notes

- The application requires Administrator privileges to modify system settings
- Tested on Windows 10 and Windows 11
- .NET 8 SDK is required for building