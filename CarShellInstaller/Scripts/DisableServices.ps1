# CarShell Installer - Disable Unnecessary Services
param(
    [string[]]$Services = @(
        "wuauserv", "BITS", "DoSvc", "SysMain", "Schedule",
        "Themes", "DiagTrack", "dmwappushservice", "WindowsSearch"
    )
)

Write-Host "Disabling unnecessary services..." -ForegroundColor Cyan
$disabled = 0
$failed = 0

foreach ($service in $Services) {
    try {
        $svc = Get-Service -Name $service -ErrorAction SilentlyContinue
        if ($svc) {
            if ($svc.Status -eq 'Running') {
                Stop-Service -Name $service -Force -ErrorAction Stop
                $svc.WaitForStatus('Stopped', (New-TimeSpan -Seconds 30))
            }
            Set-Service -Name $service -StartupType Disabled
            Write-Host "  Disabled: $service" -ForegroundColor Green
            $disabled++
        }
    } catch {
        Write-Host "  Error: $service - $($_.Exception.Message)" -ForegroundColor Red
        $failed++
    }
}
Write-Host "Done. Disabled: $disabled, Failed: $failed" -ForegroundColor Green