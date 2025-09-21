using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sephp.MessageBusRequests
{
    public class DebugOverlayRequest
    {
        public bool Show { get; set; } = true;

        public DebugOverlayRequest(bool show)
        {
            Show = show;
        }
    }
}
