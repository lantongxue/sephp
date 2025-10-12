using ReactiveUI;
using System;
using System.Diagnostics;

namespace sephp.Share.Models
{
    public class PackageProcess : ReactiveObject, IDisposable
    {
        private Process? process;

        private bool isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set => this.RaiseAndSetIfChanged(ref isRunning, value);
        }

        public ProcessStartInfo StartInfo;

        public PackageProcess(ProcessStartInfo info)
        {
            StartInfo = info;
        }

        public PackageProcess(Process p, ProcessStartInfo info)
        {
            StartInfo = info;
            p.Refresh();
            process = p;
        }

        public Process StartProcess(params string[] args)
        {
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    StartInfo.ArgumentList.Add(arg);
                }
            }

            Process p = new Process()
            {
                StartInfo = StartInfo
            };

            p.EnableRaisingEvents = true;
            p.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Debug.WriteLine($"[Output] {e.Data}");
                }
            };
            p.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Debug.WriteLine($"[Error] {e.Data}");
                }
            };
            p.Exited += (_, __) =>
            {
                IsRunning = false;
            };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            return p;
        }

        public async Task<bool> Start(params string[] args)
        {
            if(process != null)
            {
                Stop();
            }
            return await Task.Run<bool>(() =>
            {
                process = StartProcess(args);
                IsRunning = !process.HasExited;
                return IsRunning;
            });
        }

        public void Stop()
        {
            if (process == null)
            {
                return;
            }
            if (IsRunning && !process.HasExited)
            {
                process.Kill(true);
                process.WaitForExit();
                IsRunning = false;
                process = null;
            }
        }

        public async Task Restart()
        {
            Stop();
            await Task.Delay(1000);
            await Start();
        }

        public int GetPid()
        {
            if (process == null)
            {
                return 0;
            }
            return process.Id;
        }

        public static PackageProcess FindProcessById(int id, ProcessStartInfo info)
        {
            try
            {
                Process p = Process.GetProcessById(id);
                return new PackageProcess(p, info)
                {
                    IsRunning = !p.HasExited
                };
            }
            catch
            {
                return new PackageProcess(info)
                {
                    IsRunning = false
                };
            }
        }

        public void Dispose()
        {
            process?.Dispose();
        }
    }
}
