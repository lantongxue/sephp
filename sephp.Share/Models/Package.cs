using System;
using System.IO;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

namespace sephp.Share.Models;

public class Package
{
    public virtual string Id { get; set; }

    public virtual string Version { get; set; }

    [YamlIgnore]
    public static string Directory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages");
    
    public PlatformBinrary? PlatformBinrary { get; set; }

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
            return Path.Combine(Directory, executor);
        }
    }

    [YamlIgnore]
    public PackageProcess Process {  get; set; }
}

public class PlatformBinrary
{
    public string Windows { get; set; } = "";

    public string Linux { get; set; } = "";

    public string Darwin { get; set; } = "";
}