using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace CarShellInstaller.Core
{
    public class ServiceManager
    {
        private readonly string[] _unnecessaryServices =
        {
            "wuauserv",
            "BITS",
            "DoSvc",
            "SysMain",
            "Schedule",
            "Themes",
            "DiagTrack",
            "dmwappushservice",
            "WindowsSearch",
            "WSearch",
            "OneSyncSvc",
            "MapsBroker",
            "lfsvc",
            "SharedAccess",
            "iphlpsvc",
            "HomeGroupProvider",
            "HomeGroupListener"
        };

        private readonly string[] _essentialServices =
        {
            "RpcSs",
            "DcomLaunch",
            "RpcEptMapper",
            "PlugPlay",
            "Power",
            "AudioSrv",
            "AudioEndpointBuilder",
            "WinDefend",
            "EventLog",
            "NlaSvc",
            "Dnscache",
            "Dhcp",
            "LanmanWorkstation",
            "LanmanServer"
        };


        public bool DisableUnnecessaryServices()
        {
            try
            {
                foreach (var name in _unnecessaryServices)
                {
                    DisableService(name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disabling services: " + ex.Message);
                return false;
            }
        }


        public bool EnableEssentialServices()
        {
            try
            {
                foreach (var name in _essentialServices)
                {
                    EnableService(name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enabling services: " + ex.Message);
                return false;
            }
        }


        public bool DisableService(string serviceName)
        {
            try
            {
                using (var svc = new ServiceController(serviceName))
                {
                    if (svc.Status == ServiceControllerStatus.Running)
                    {
                        svc.Stop();
                        svc.WaitForStatus(
                            ServiceControllerStatus.Stopped,
                            TimeSpan.FromSeconds(30));
                    }
                }

                return SetServiceStartupType(serviceName, "disabled");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error disabling " + serviceName + ": " + ex.Message);

                return false;
            }
        }


        public bool EnableService(string serviceName)
        {
            try
            {
                if (!SetServiceStartupType(serviceName, "auto"))
                    return false;

                using (var svc = new ServiceController(serviceName))
                {
                    if (svc.Status == ServiceControllerStatus.Stopped)
                    {
                        svc.Start();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error enabling " + serviceName + ": " + ex.Message);

                return false;
            }
        }


        private bool SetServiceStartupType(string serviceName, string startType)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $"config \"{serviceName}\" start= {startType}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using var process = Process.Start(psi);

                process?.WaitForExit();

                return process?.ExitCode == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error changing startup type for {serviceName}: {ex.Message}");

                return false;
            }
        }


        public bool RestoreDefaultServices()
        {
            try
            {
                foreach (var name in _unnecessaryServices)
                {
                    EnableService(name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Error restoring services: " + ex.Message);

                return false;
            }
        }
    }
}