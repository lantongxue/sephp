using System;
using System.IO;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

namespace sephp.Share.Models;

public class Package
{
    public string Id { get; set; } = "";

    public string Version { get; set; } = "";

    [YamlIgnore]
    public string Directory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages", Id, Version);
    
    public PackageBinrary BinraryDirectory { get; set; }

    [YamlIgnore]
    public string Executor
    {
        get
        {
            string executor = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                executor = BinraryDirectory.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                executor = BinraryDirectory.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                executor = BinraryDirectory.Darwin;
            }
            return Path.Combine(Directory, executor);
        }
    }

    [YamlIgnore]
    public PackageProcess? Process {  get; set; }

}

public class PackageBinrary
{
    public string Windows { get; set; }

    public string Linux { get; set; }

    public string Darwin { get; set; }
}