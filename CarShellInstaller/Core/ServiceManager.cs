using System;
using System.Diagnostics;
using System.Threading;


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
                        if (IsServiceRunning(name))
                        {
                            RunSc($"stop \"{name}\"");
                            WaitForState(name, wantRunning: false, timeoutSeconds: 30);
                        }

                        // Change start type to disabled using sc.exe
                        SetServiceStartType(name, "disabled");
                    }
                    catch (Exception ex)
                    {
                        // Локально логируем, но продолжаем обработку остальных сервисов
                        DebugWrite($"DisableUnnecessaryServices: {name} error: {ex.Message}");
                    }
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
                           if (!IsServiceRunning(name))
                        {
                            RunSc($"start \"{name}\"");
                            WaitForState(name, wantRunning: true, timeoutSeconds: 30);
                        }

                        // Set start type to automatic
                        SetServiceStartType(name, "auto");
                    }
                    catch (Exception ex)
                    {
                        // Локально логируем, но продолжаем обработку остальных сервисов
                        DebugWrite($"EnableEssentialServices: {name} error: {ex.Message}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        private void SetServiceStartType(string serviceName, string startMode)
        {
            try
            {
                RunSc($"config \"{serviceName}\" start= {startMode}");
            }
            catch (Exception ex)
            {
                DebugWrite(ex.ToString());
            }
        }

        private static string RunSc(string arguments, int timeoutMs = 30000)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "sc.exe",
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var proc = Process.Start(psi))
            {
                if (proc == null) return string.Empty;
                var output = proc.StandardOutput.ReadToEnd();
                var error = proc.StandardError.ReadToEnd();
                proc.WaitForExit(timeoutMs);
                return (output ?? string.Empty) + Environment.NewLine + (error ?? string.Empty);
            }
        }

        private static bool IsServiceRunning(string serviceName)
        {
            try
            {
                var output = RunSc($"query \"{serviceName}\"");
                var code = ParseStateCode(output);
                // 4 == RUNNING
                return code.HasValue && code.Value == 4;
            }
            catch
            {
                return false;
            }
        }

        private static int? ParseStateCode(string scOutput)
        {
            if (string.IsNullOrEmpty(scOutput)) return null;
            var lines = scOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in lines)
            {
                var line = raw.TrimStart();
                // Ищем строку, начинающуюся с "STATE" (независимо от регистра)
                if (line.StartsWith("STATE", StringComparison.OrdinalIgnoreCase))
                {
                    var parts = line.Split(new[] { ':' }, 2);
                    if (parts.Length < 2) continue;
                    var after = parts[1];
                    var tokens = after.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tok in tokens)
                    {
                        if (int.TryParse(tok, out int code))
                        {
                            return code;
                        }
                    }
                }
            }
            return null;
        }

        private static bool WaitForState(string serviceName, bool wantRunning, int timeoutSeconds)
        {
            var sw = Stopwatch.StartNew();
            var desired = wantRunning ? 4 : 1; // 4 = RUNNING, 1 = STOPPED
            while (sw.Elapsed.TotalSeconds < timeoutSeconds)
            {
                var code = ParseStateCode(RunSc($"query \"{serviceName}\""));
                if (code.HasValue && code.Value == desired) return true;
                Thread.Sleep(500);
            }
            return false;
        }

        private static void DebugWrite(string message)
        {
            try
            {
                // Пытаемся записать отладочную информацию безопасно
                Debug.WriteLine(message);
            }
            catch { }
        }
    }
}
