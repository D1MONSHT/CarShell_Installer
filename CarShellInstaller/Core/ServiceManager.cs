using System;
using System.ServiceProcess;

namespace CarShellInstaller.Core
{
    public class ServiceManager
    {
        private readonly string[] _unnecessaryServices = {
            "wuauserv",       // Windows Update
            "BITS",           // Background Intelligent Transfer Service
            "DoSvc",          // Delivery Optimization
            "SysMain",        // Superfetch
            "Schedule",       // Task Scheduler
            "Themes",         // Themes
            "DiagTrack",      // Diagnostic Tracking
            "dmwappushservice", // DMWAPPushService
            "WindowsSearch",   // Windows Search
            "WSearch",        // Windows Search (alternative)
            "OneSyncSvc",     // OneSync
            "MapsBroker",     // Maps Broker
            "lfsvc",         // Location Framework
            "SharedAccess",   // Internet Connection Sharing
            "iphlpsvc",      // IP Helper
            "HomeGroupProvider", // HomeGroup Provider
            "HomeGroupListener" // HomeGroup Listener
        };

        private readonly string[] _essentialServices = {
            "RpcSs",              // RPC
            "DcomLaunch",        // DCOM Server Process Launcher
            "RpcEptMapper",      // RPC Endpoint Mapper
            "PlugPlay",          // Plug and Play
            "Power",             // Power
            "AudioSrv",          // Windows Audio
            "AudioEndpointBuilder", // Windows Audio Endpoint Builder
            "WinDefend",         // Windows Defender
            "Schedule",          // Task Scheduler (keep enabled)
            "EventLog",          // Windows Event Log
            "SysMain",           // Superfetch (keep enabled)
            "NlaSvc",            // Network Location Awareness
            "Dnscache",          // DNS Client
            "Dhcp",              // DHCP Client
            "LanmanWorkstation", // Workstation
            "LanmanServer",      // Server
            "Tcpip",             // TCP/IP Protocol Driver
            "afd",               // Ancillary Function Driver
            "NetBT",             // NetBIOS over TCP/IP
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
                        svc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }
                    svc.StartType = ServiceStartMode.Disabled;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disabling " + serviceName + ": " + ex.Message);
                return false;
            }
        }

        public bool EnableService(string serviceName)
        {
            try
            {
                using (var svc = new ServiceController(serviceName))
                {
                    svc.StartType = ServiceStartMode.Automatic;
                    if (svc.Status == ServiceControllerStatus.Stopped)
                    {
                        svc.Start();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enabling " + serviceName + ": " + ex.Message);
                return false;
            }
        }

        public bool RestoreDefaultServices()
        {
            try
            {
                // Re-enable previously disabled services
                foreach (var name in _unnecessaryServices)
                {
                    EnableService(name);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error restoring services: " + ex.Message);
                return false;
            }
        }
    }
}