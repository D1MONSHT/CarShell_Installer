using Microsoft.Win32;
using System;

namespace CarShellInstaller.Core
{
    public class ShellManager
    {
        public bool ConfigureForCarShell()
        {
            try
            {
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "Shell", "explorer.exe", RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool RestoreDefaultShell()
        {
            try
            {
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "Shell", "explorer.exe", RegistryValueKind.String);
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