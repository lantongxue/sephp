using sephp.Nginx.Parser.NginxConfig;
using sephp.Nginx.Parser.NginxConfig.Directive;
using sephp.Share.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace sephp.Nginx.Models
{
    public class NginxPackage : Package
    {
        public override string Id { get; set; } = "nginx";

        ConfigRoot? _configRoot;

        public override async Task Reload()
        {
            var info = GetStartInfo("-s", "reload");
            PackageProcess p = new PackageProcess(info);
            await p.Start();
        }

        public ConfigRoot? GetConfig(bool force = false)
        {
            if (_configRoot == null || force)
            {
                _configRoot = ConfigParser.Parse(Path.Combine(PackageDirectory, "conf", "nginx.conf"));
            }
            return _configRoot;
        }

        public string GetAccessLog(string hostname = "")
        {
            var ast = GetConfig();
            var httpDirective = ast?.Find("http");
            if (httpDirective == null)
            {
                throw new Exception("Config error: [http] directive not exists.");
            }
            if (string.IsNullOrEmpty(hostname))
            {
                var httpAccessLog = httpDirective?.Block?.Find("access_log");
                if (httpAccessLog == null)
                {
                    return Path.Combine(PackageDirectory, "logs", "access.log");
                }
                if (Path.IsPathRooted(httpAccessLog.Args[0]))
                {
                    return httpAccessLog.Args[0];
                }

                return Path.Combine(PackageDirectory, httpAccessLog.Args[0]);
            }

            return FindLogFile(httpDirective, "access_log", hostname);
        }

        public string GetErrorLog(string hostname = "")
        {
            var ast = GetConfig();
            if (string.IsNullOrEmpty(hostname))
            {
                var errorLogDirective = ast?.Find("error_log");
                if (errorLogDirective == null)
                {
                    return Path.Combine(PackageDirectory, "logs", "error.log");
                }

                var log = errorLogDirective.Args[0];
                if (Path.IsPathRooted(log))
                {
                    return log;
                }

                return Path.Combine(PackageDirectory, log);
            }

            var httpDirective = ast?.Find("http");
            if (httpDirective == null)
            {
                throw new Exception("Config error: [http] directive not exists.");
            }

            return FindLogFile(httpDirective, "error_log", hostname);
        }

        protected string FindLogFile(ConfigDirective httpDirective, string type, string hostname)
        {
            IEnumerable<ServerDirective>? servers = httpDirective.Block?.FindDirectives<ServerDirective>();
            if (servers == null)
            {
                return "";
            }

            var server = servers.Where(s => s.Args.Contains(hostname)).FirstOrDefault();
            if (server == null)
            {
                return "";
            }

            var logDirective = server.Block?.Find(type);
            if (logDirective == null)
            {
                return "";
            }

            if (Path.IsPathRooted(logDirective.Args[0]))
            {
                return logDirective.Args[0];
            }

            return Path.Combine(PackageDirectory, logDirective.Args[0]);
        }
    }
}
