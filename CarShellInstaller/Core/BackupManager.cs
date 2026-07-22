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
                string backupPath = Path.Combine(_backupDirectory, $"Backup_{timestamp}");

                Directory.CreateDirectory(backupPath);

                string regBackupPath = Path.Combine(backupPath, "Registry");
                Directory.CreateDirectory(regBackupPath);

                ExportRegistryKey(Registry.LocalMachine, "SOFTWARE", Path.Combine(regBackupPath, "SOFTWARE.reg"));
                ExportRegistryKey(Registry.LocalMachine, "SYSTEM", Path.Combine(regBackupPath, "SYSTEM.reg"));
                ExportRegistryKey(Registry.CurrentUser, "", Path.Combine(regBackupPath, "USER.reg"));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool CreateRestorePoint(string description)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-Command \"Checkpoint-Computer -Description '{description}' -RestorePointType MODIFY_SETTINGS\"",
                    UseShellExecute = true,
                    Verb = "runas"
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private void ExportRegistryKey(RegistryKey root, string subKey, string filePath)
        {
            try
            {
                string keyPath = string.IsNullOrEmpty(subKey)
                    ? root.Name
                    : $"{root.Name}\\{subKey}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = "reg.exe",
                    Arguments = $"export \"{keyPath}\" \"{filePath}\" /y",
                    CreateNoWindow = true,
                    UseShellExecute = false
                })?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
