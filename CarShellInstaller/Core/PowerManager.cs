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
                    @"Control PanelDesktop",
                    "ScreenSaveActive", "0", RegistryValueKind.String);
                
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "ScreenSaveTimeOut", "600", RegistryValueKind.String);
                
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "MonitorTimeout", "600", RegistryValueKind.String);
                
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetControlPower",
                    "HiberFileSizePercent", 0, RegistryValueKind.DWord);
                
                var guid1 = "54533251-82be-4824-96c1-47b60b740d00";
                var sub1 = "75b0ae3f-bce0-45a7-8c89-c9611c25e100";
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetControlPowerPowerSettings" + guid1 + @"" + sub1,
                    "Attributes", 2, RegistryValueKind.DWord);
                
                var guid2 = "238C9FA8-0AAD-41ED-83F4-97BE242C8F20";
                var sub2 = "7bc4a8f0-d8ec-4e18-b0e0-3e1832435130";
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetControlPowerPowerSettings" + guid2 + @"" + sub2,
                    "Attributes", 2, RegistryValueKind.DWord);
                
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
            try
            {
                OptimizeForCar();
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "EnableLUA", 0, RegistryValueKind.DWord);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool RestoreDefaultPower()
        {
            try
            {
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "ScreenSaveActive", "1", RegistryValueKind.String);
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "ScreenSaveTimeOut", "600", RegistryValueKind.String);
                SetRegistryValue(Registry.CurrentUser,
                    @"Control PanelDesktop",
                    "MonitorTimeout", "600", RegistryValueKind.String);
                SetRegistryValue(Registry.LocalMachine,
                    @"SYSTEMCurrentControlSetControlPower",
                    "HiberFileSizePercent", 75, RegistryValueKind.DWord);
                SetRegistryValue(Registry.LocalMachine,
                    @"SOFTWAREMicrosoftWindowsCurrentVersionPoliciesSystem",
                    "EnableLUA", 1, RegistryValueKind.DWord);
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