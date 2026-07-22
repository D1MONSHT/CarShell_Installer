using Microsoft.Win32;
using System;

namespace CarShellInstaller.Core
{
    public class PowerManager
    {
        public bool OptimizeForCar()
        {
            try
            {
                SetRegistryValue(Registry.CurrentUser,
                    @"Control Panel\Desktop",
                    "ScreenSaveActive", "0", RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool OptimizeForKiosk()
        {
            return OptimizeForCar();
        }

        public bool RestoreDefaultPower()
        {
            try
            {
                SetRegistryValue(Registry.CurrentUser,
                    @"Control Panel\Desktop",
                    "ScreenSaveActive", "1", RegistryValueKind.String);
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