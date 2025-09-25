using System;
using System.IO;
using System.Runtime.InteropServices;
using YamlDotNet.Serialization;

namespace sephp.Models;

public class Package
{
    public string Id { get; set; }
    
    public string Version { get; set; }

    [YamlIgnore]
    public string Directory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packages", Id, Version);
    
    public string BinraryDirectory { get; set; }

    [YamlIgnore]
    public string Executor
    {
        get
        {
            string executor = Id;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                executor += ".exe";
            }
            return Path.Combine(Directory, executor);
        }
    }

    public Version GetVersion()
    {
        return new Version(Version);
    }
}