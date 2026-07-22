# CarShell Installer - Windows Optimization
param(
    [switch]$DisableVisualEffects = $true,
    [switch]$DisableIndexing = $true,
    [switch]$DisableHibernation = $true,
    [switch]$DisableSleep = $true
)

Write-Host "Optimizing Windows..." -ForegroundColor Cyan

if ($DisableVisualEffects) {
    try { Set-ItemProperty -Path "HKLM:SOFTWAREMicrosoftWindowsCurrentVersionExplorerVisualEffects" -Name "VisualFXSetting" -Value 2 -Type DWORD
    Write-Host "  Visual effects disabled" -ForegroundColor Green } catch { Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red }
}

if ($DisableIndexing) {
    try { Stop-Service -Name "WSearch" -Force -ErrorAction SilentlyContinue; Set-Service -Name "WSearch" -StartupType Disabled -ErrorAction SilentlyContinue
    Write-Host "  Indexing disabled" -ForegroundColor Green } catch { Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red }
}

if ($DisableHibernation) {
    try { powercfg /h off; Write-Host "  Hibernation disabled" -ForegroundColor Green } catch { Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red }
}

if ($DisableSleep) {
    try { powercfg /x -standby-timeout-ac 0; powercfg /x -standby-timeout-dc 0
    Write-Host "  Sleep disabled" -ForegroundColor Green } catch { Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red }
}

Write-Host "Optimization complete!" -ForegroundColor Green