using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Addon
{
    public class Information
    {
        public string? Name { get; set; }

        public string? Author { get; set; }

        public string? Description { get; set; }

        public string? Website { get; set; }

        public Version? Version { get; set; } = new Version(1, 0, 0);
    }
}
