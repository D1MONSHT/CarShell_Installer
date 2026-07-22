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
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableTaskMgr", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREPoliciesMicrosoftWindowsSystem",
                    "DisableCMD", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWAREPoliciesMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "DisableRegistryTools", 1, RegistryValueKind.DWord);
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