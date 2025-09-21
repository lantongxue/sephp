using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Models
{
    public class AppSettings
    {
        public bool DebugOverlay { get; set; } = false;

        public double Width { get; set; } = 1200;

        public double Height { get; set; } = 600;
    }
}
