# CarShell Installer - Windows Restoration
param(
    [switch]$RestoreServices = $true,
    [switch]$RestorePower = $true
)

Write-Host "Restoring Windows..." -ForegroundColor Cyan

if ($RestoreServices) {
    @("wuauserv", "BITS", "DoSvc", "SysMain", "Schedule", "Themes") | ForEach-Object {
        try {
            $svc = Get-Service -Name $_ -ErrorAction SilentlyContinue
            if ($svc) {
                Set-Service -Name $_ -StartupType Automatic
                if ($svc.Status -eq 'Stopped') Start-Service -Name $_ -ErrorAction SilentlyContinue
            }
        } catch { }
    }
    Write-Host "  Services restored" -ForegroundColor Green
}

if ($RestorePower) {
    try { powercfg /h on; powercfg /x -standby-timeout-ac 20; powercfg /x -standby-timeout-dc 20
    Write-Host "  Power settings restored" -ForegroundColor Green } catch { Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red }
}

Write-Host "Restoration complete!" -ForegroundColor Green