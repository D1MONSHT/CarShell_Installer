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
                
                // Export critical registry keys
                ExportRegistryKey(Registry.LocalMachine, "SOFTWARE", Path.Combine(regBackupPath, "SOFTWARE.reg"));
                ExportRegistryKey(Registry.LocalMachine, "SYSTEM", Path.Combine(regBackupPath, "SYSTEM.reg"));
                ExportRegistryKey(Registry.CurrentUser, string.Empty, Path.Combine(regBackupPath, "USER.reg"));
                
                // Export services configuration
                ExportRegistryKey(Registry.LocalMachine, "SYSTEMCurrentControlSetServices", Path.Combine(regBackupPath, "Services.reg"));
                
                // Export Windows policies
                ExportRegistryKey(Registry.LocalMachine, "SOFTWAREMicrosoftWindowsCurrentVersionPolicies", Path.Combine(regBackupPath, "Policies.reg"));
                
                // Save backup info
                File.WriteAllText(Path.Combine(backupPath, "backup.info"), 
                    "CarShell Installer Backup
" +
                    "Created: " + DateTime.Now.ToString() + "
" +
                    "Mode: Full System Backup");
                
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
                System.Diagnostics.Process.Start("powershell", 
                    "-Command "Checkpoint-Computer -Description '" + safeDescription + "' -RestorePointType MODIFY_SETTINGS -ErrorAction SilentlyContinue"",
                    new System.Diagnostics.ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });
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
                        System.Diagnostics.Process.Start("reg", "import "" + regFile + """,
                            new System.Diagnostics.ProcessStartInfo
                            {
                                CreateNoWindow = true,
                                UseShellExecute = false
                            })?.WaitForExit();
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

        private void ExportRegistryKey(RegistryKey root, string subKey, string filePath)
        {
            try
            {
                string keyPath = string.IsNullOrEmpty(subKey) ? root.Name : Path.Combine(root.Name, subKey);
                System.Diagnostics.Process.Start("reg", 
                    "export "" + keyPath + "" "" + filePath + "" /y",
                    new System.Diagnostics.ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false
                    })?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error exporting registry: " + ex.Message);
            }
        }
    }
}