using ReactiveUI;
using System;
using System.Collections.Generic;
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
            private set => this.RaiseAndSetIfChanged(ref isRunning, value);
        }

        public PackageProcess(string executor, IEnumerable<string> args)
        {
            process = new Process
            {
                StartInfo = new ProcessStartInfo(executor, args)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            process.Exited += (_, __) => IsRunning = false;
        }

        public PackageProcess(Process p)
        {
            process = p;
        }

        public PackageProcess()
        {
        }

        public async Task<bool> Start()
        {
            if (process == null)
            {
                return false;
            }
            await Task.Run(() =>
            {
                IsRunning = process.Start();
            });

            return IsRunning;
        }

        public void KillProcess()
        {
            if (process == null)
            {
                return;
            }
            if (IsRunning && !process.HasExited)
            {
                process.Kill();
                IsRunning = false;
            }
        }

        public async Task Restart()
        {
            KillProcess();
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

        public static PackageProcess FindProcessById(int id)
        {
            try
            {
                var p = Process.GetProcessById(id);
                return new PackageProcess(p)
                {
                    IsRunning = !p.HasExited
                };
            }
            catch
            {
                return new PackageProcess()
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
