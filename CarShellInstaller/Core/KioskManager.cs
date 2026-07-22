using Microsoft.Win32;
using System;

namespace CarShellInstaller.Core
{
    public class KioskManager
    {
        public bool ConfigureKioskMode()
        {
            try
            {
                // Security settings
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableTaskMgr", 1, RegistryValueKind.DWord);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableChangePassword", 1, RegistryValueKind.DWord);
                
                // Disable Ctrl+Alt+Del
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                
                // Disable Registry Editor
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableRegistryTools", 1, RegistryValueKind.DWord);
                
                // Disable Command Prompt
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREPoliciesMicrosoftWindowsSystem",
                    "DisableCMD", 2, RegistryValueKind.DWord);
                
                // Hide desktop icons and taskbar
                SetRegistryValue(Registry.CurrentUser,
                    @"SoftwareMicrosoftWindowsCurrentVersionPoliciesExplorer",
                    "NoDesktop", 1, RegistryValueKind.DWord);
                
                SetRegistryValue(Registry.CurrentUser,
                    @"SoftwareMicrosoftWindowsCurrentVersionPoliciesExplorer",
                    "NoTrayContextMenu", 1, RegistryValueKind.DWord);
                
                // Disable access to drives
                SetRegistryValue(Registry.CurrentUser,
                    @"SoftwareMicrosoftWindowsCurrentVersionPoliciesExplorer",
                    "NoDrives", 0xFFFFFFFF, RegistryValueKind.DWord);
                
                // Disable Control Panel
                SetRegistryValue(Registry.CurrentUser,
                    @"SoftwareMicrosoftWindowsCurrentVersionPoliciesExplorer",
                    "NoControlPanel", 1, RegistryValueKind.DWord);
                
                // Disable Settings app
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREPoliciesMicrosoftWindowsImmersiveShell",
                    "UseActionCenterExperience", 0, RegistryValueKind.DWord);
                
                // Disable Windows Update access
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREPoliciesMicrosoftWindowsWindowsUpdate",
                    "DoNotConnectToWindowsUpdateInternetLocations", 1, RegistryValueKind.DWord);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREPoliciesMicrosoftWindowsWindowsUpdateAU",
                    "NoAutoUpdate", 1, RegistryValueKind.DWord);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool ConfigureAutoLogin(string username, string password)
        {
            try
            {
                // Note: This stores password in plain text in registry
                // For security, consider using LSA or other secure methods
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "AutoAdminLogon", 1, RegistryValueKind.DWord);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "DefaultUsername", username, RegistryValueKind.String);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "DefaultPassword", password, RegistryValueKind.String);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "ForceAutoLogon", 1, RegistryValueKind.DWord);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        private void SetRegistryValue(RegistryKey root, string path, string name, object value, RegistryValueKind kind)
        {
            using (var key = root.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if (key != null) key.SetValue(name, value, kind);
            }
        }
    }
}