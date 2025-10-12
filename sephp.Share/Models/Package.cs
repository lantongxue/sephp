using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

namespace sephp.Share.Models;

public class Package : ReactiveObject
{
    public virtual string Id { get; set; }

    public virtual string Version { get; set; }

    public virtual PlatformBinrary? PlatformBinrary { get; set; }

    [YamlIgnore]
    public static string Directory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages");

    [YamlIgnore]
    public string PackageDirectory => Path.Combine(Directory, Id, Version);

    [YamlIgnore]
    public string Executor
    {
        get
        {
            if(PlatformBinrary == null)
            {
                return "";
            }
            string executor = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                executor = PlatformBinrary.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                executor = PlatformBinrary.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                executor = PlatformBinrary.Darwin;
            }
            return Path.Combine(PackageDirectory, executor);
        }
    }

    private PackageProcess? _process;
    [YamlIgnore]
    public PackageProcess? Process
    {
        get => _process;
        set => this.RaiseAndSetIfChanged(ref _process, value);
    }

    public void Init(int pid)
    {
        ProcessStartInfo info = GetStartInfo();
        if (pid > 0)
        {
            Process = PackageProcess.FindProcessById(pid, info);
        }
        else
        {
            Process = new PackageProcess(info);
        }
    }

    public virtual ProcessStartInfo GetStartInfo(params string[] args)
    {
        return new ProcessStartInfo(Executor, args)
        {
            WorkingDirectory = PackageDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };
    }

    public virtual async Task Start()
    {
        await Process!.Start();
    }

    public virtual void Stop()
    {
        Process!.Stop();
    }

    public virtual async Task Restart()
    {
        await Process!.Restart();
    }

    public virtual Task Reload()
    {
        throw new NotImplementedException();
    }

}

public class PlatformBinrary
{
    public string Windows { get; set; } = "";

    public string Linux { get; set; } = "";

    public string Darwin { get; set; } = "";
}