using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace CarShellInstaller.Core
{
    public class BackupManager
    {
        private readonly string _backupDirectory;

        public BackupManager()
        {
            _backupDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "CarShellInstaller",
                "Backups");

            Directory.CreateDirectory(_backupDirectory);
        }

        public bool CreateBackup()
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupPath = Path.Combine(_backupDirectory, "Backup_" + timestamp);

                Directory.CreateDirectory(backupPath);

                string regBackupPath = Path.Combine(backupPath, "Registry");
                Directory.CreateDirectory(regBackupPath);

                ExportRegistryKey(
                    Registry.LocalMachine,
                    "SOFTWARE",
                    Path.Combine(regBackupPath, "SOFTWARE.reg"));

                ExportRegistryKey(
                    Registry.LocalMachine,
                    "SYSTEM",
                    Path.Combine(regBackupPath, "SYSTEM.reg"));

                ExportRegistryKey(
                    Registry.CurrentUser,
                    string.Empty,
                    Path.Combine(regBackupPath, "USER.reg"));

                ExportRegistryKey(
                    Registry.LocalMachine,
                    @"SYSTEM\CurrentControlSet\Services",
                    Path.Combine(regBackupPath, "Services.reg"));

                ExportRegistryKey(
                    Registry.LocalMachine,
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies",
                    Path.Combine(regBackupPath, "Policies.reg"));

                File.WriteAllText(
                    Path.Combine(backupPath, "backup.info"),
                    $"CarShell Installer Backup\nCreated: {DateTime.Now}\nMode: Full System Backup");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating backup: " + ex.Message);
                return false;
            }
        }


        public bool CreateRestorePoint(string description)
        {
            try
            {
                string safeDescription = description.Replace("'", "''");

                string command =
                    $"Checkpoint-Computer -Description '{safeDescription}' " +
                    "-RestorePointType MODIFY_SETTINGS " +
                    "-ErrorAction SilentlyContinue";

                var startInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "-Command \"" + command + "\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                Process.Start(startInfo)?.WaitForExit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating restore point: " + ex.Message);
                return false;
            }
        }


        public bool RestoreBackup(string backupPath)
        {
            try
            {
                string regPath = Path.Combine(backupPath, "Registry");

                if (Directory.Exists(regPath))
                {
                    string[] regFiles = Directory.GetFiles(regPath, "*.reg");

                    foreach (string regFile in regFiles)
                    {
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = "reg",
                            Arguments = $"import \"{regFile}\"",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        };

                        Process.Start(startInfo)?.WaitForExit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error restoring backup: " + ex.Message);
                return false;
            }
        }


        private void ExportRegistryKey(
            RegistryKey root,
            string subKey,
            string filePath)
        {
            try
            {
                string keyPath = string.IsNullOrEmpty(subKey)
                    ? root.Name
                    : root.Name + "\\" + subKey;

                var startInfo = new ProcessStartInfo
                {
                    FileName = "reg",
                    Arguments = $"export \"{keyPath}\" \"{filePath}\" /y",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                Process.Start(startInfo)?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error exporting registry: " + ex.Message);
            }
        }
    }
}