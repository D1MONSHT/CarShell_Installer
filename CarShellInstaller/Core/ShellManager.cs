using Microsoft.Win32;
using System;
using System.IO;

namespace CarShellInstaller.Core
{
    public class ShellManager
    {
        private string _originalShell = "explorer.exe";
        private string _carShellPath = "CarShell.exe";

        public bool ConfigureForCarShell(string carShellPath = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(carShellPath))
                {
                    _carShellPath = Path.GetFileName(carShellPath);
                }
                
                // Check if CarShell exists
                if (!File.Exists(_carShellPath) && !File.Exists(Path.Combine(Environment.SystemDirectory, _carShellPath)))
                {
                    Console.WriteLine("Warning: CarShell.exe not found. Using explorer.exe as fallback.");
                    return false;
                }
                
                // Backup original shell
                _originalShell = GetCurrentShell();
                
                // Set CarShell as default shell
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon",
                    "Shell", _carShellPath, RegistryValueKind.String);
                
                // Also set in Wow6432Node for 64-bit systems
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREWOW6432NodeMicrosoftWindows NTCurrentVersionWinlogon",
                    "Shell", _carShellPath, RegistryValueKind.String);
                
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
                    "Shell", _originalShell, RegistryValueKind.String);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREWOW6432NodeMicrosoftWindows NTCurrentVersionWinlogon",
                    "Shell", _originalShell, RegistryValueKind.String);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public string GetCurrentShell()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWAREMicrosoftWindows NTCurrentVersionWinlogon"))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("Shell");
                        if (value != null)
                        {
                            return value.ToString();
                        }
                    }
                }
                return "explorer.exe";
            }
            catch
            {
                return "explorer.exe";
            }
        }

        public bool SetShell(string shellPath)
        {
            try
            {
                _carShellPath = Path.GetFileName(shellPath);
                return ConfigureForCarShell(shellPath);
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