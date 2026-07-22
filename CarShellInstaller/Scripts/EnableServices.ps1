# CarShell Installer - Enable Essential Services
param(
    [string[]]$Services = @(
        "RpcSs", "DcomLaunch", "RpcEptMapper", "PlugPlay",
        "Power", "AudioSrv", "AudioEndpointBuilder"
    )
)

Write-Host "Enabling essential services..." -ForegroundColor Cyan
$enabled = 0

foreach ($service in $Services) {
    try {
        $svc = Get-Service -Name $service -ErrorAction SilentlyContinue
        if ($svc) {
            Set-Service -Name $service -StartupType Automatic
            if ($svc.Status -eq 'Stopped') Start-Service -Name $service -ErrorAction SilentlyContinue
            Write-Host "  Enabled: $service" -ForegroundColor Green
            $enabled++
        }
    } catch {
        Write-Host "  Error: $service - $($_.Exception.Message)" -ForegroundColor Red
    }
}
Write-Host "Done. Enabled: $enabled" -ForegroundColor Green