using System;
using System.ServiceProcess;

namespace CarShellInstaller.Core
{
    public class ServiceManager
    {
        public bool DisableUnnecessaryServices()
        {
            try
            {
                var services = new string[] { "wuauserv", "BITS", "DoSvc", "SysMain", "Schedule", "Themes", "DiagTrack", "dmwappushservice" };
                foreach (var name in services)
                {
                    try
                    {
                        using (var svc = new ServiceController(name))
                        {
                            if (svc.Status == ServiceControllerStatus.Running)
                            {
                                svc.Stop();
                                svc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                            }
                            svc.StartType = ServiceStartMode.Disabled;
                        }
                    }
                    catch { }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool EnableEssentialServices()
        {
            try
            {
                var services = new string[] { "RpcSs", "DcomLaunch", "RpcEptMapper", "PlugPlay", "Power", "AudioSrv", "AudioEndpointBuilder" };
                foreach (var name in services)
                {
                    try
                    {
                        using (var svc = new ServiceController(name))
                        {
                            svc.StartType = ServiceStartMode.Automatic;
                            if (svc.Status == ServiceControllerStatus.Stopped) svc.Start();
                        }
                    }
                    catch { }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}