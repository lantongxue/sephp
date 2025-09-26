using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using sephp.Share.Services.Interfaces;

namespace sephp.Share.Services.Config
{
    public class YamlConfigService<T> : IConfigService<T> where T : new()
    {
        private readonly string _filePath;
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serializer;

        public T Settings { get; private set; } = new();

        public event Action<T>? OnChanged;

        public YamlConfigService(string filePath)
        {
            _filePath = filePath;
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            _serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            Load();
            WatchFile();
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                var yaml = File.ReadAllText(_filePath);
                Settings = _deserializer.Deserialize<T>(yaml) ?? new();
            }
            else
            {
                Settings = new();
                Save();
            }
            OnChanged?.Invoke(Settings);
        }

        public void Save()
        {
            var yaml = _serializer.Serialize(Settings);
            File.WriteAllText(_filePath, yaml);
        }

        private void WatchFile()
        {
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(_filePath)!, Path.GetFileName(_filePath))
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (_, __) =>
            {
                try { Load(); } catch { /* ignore parse errors */ }
            };
            watcher.EnableRaisingEvents = true;
        }
    }

}
