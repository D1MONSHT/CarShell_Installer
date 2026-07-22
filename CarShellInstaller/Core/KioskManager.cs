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
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "DisableTaskMgr", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWARE\Policies\Microsoft\Windows\System",
                    "DisableCMD", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.CurrentUser,
                    @"SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Policies\System",
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