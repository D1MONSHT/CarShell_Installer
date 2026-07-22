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
                SetRegistryValue(Registry.LocalMachine, 
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update",
                    "AUOptions", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEM\CurrentControlSet\Services\wuauserv",
                    "Start", 4, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEM\CurrentControlSet\Control\PriorityControl",
                    "Win32PrioritySeparation", 26, RegistryValueKind.DWord);
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
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "DisableLockWorkstation", 1, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    "DisableTaskMgr", 1, RegistryValueKind.DWord);
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