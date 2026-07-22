using Microsoft.Win32;
using System;
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
                "CarShellInstaller", "Backups");
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
                ExportRegistryKey(Registry.LocalMachine, "SOFTWARE", Path.Combine(regBackupPath, "SOFTWARE.reg"));
                ExportRegistryKey(Registry.LocalMachine, "SYSTEM", Path.Combine(regBackupPath, "SYSTEM.reg"));
                ExportRegistryKey(Registry.CurrentUser, string.Empty, Path.Combine(regBackupPath, "USER.reg"));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool CreateRestorePoint(string description)
        {
            try
            {
                System.Diagnostics.Process.Start("powershell", 
                    "-Command "Checkpoint-Computer -Description '"" + description + ""' -RestorePointType MODIFY_SETTINGS"");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        private void ExportRegistryKey(RegistryKey root, string subKey, string filePath)
        {
            try
            {
                string keyPath = string.IsNullOrEmpty(subKey) ? root.Name : root.Name + "\" + subKey;
                System.Diagnostics.Process.Start("reg", "export "" + keyPath + "" "" + filePath + "" /y")?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}