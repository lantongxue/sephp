using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace sephp.Monitor
{
    public class Network
    {
        private long _lastBytesSent;
        private long _lastBytesReceived;
        private DateTime _lastCheckTime;

        public Network()
        {
            _lastCheckTime = DateTime.UtcNow;
            (_lastBytesSent, _lastBytesReceived) = GetCurrentBytes();
        }

        public (double uploadKbps, double downloadKbps) GetNetworkUsage()
        {
            var now = DateTime.UtcNow;
            var (sent, received) = GetCurrentBytes();

            var elapsedSec = (now - _lastCheckTime).TotalSeconds;
            var upload = (sent - _lastBytesSent) / 1024.0 / elapsedSec;
            var download = (received - _lastBytesReceived) / 1024.0 / elapsedSec;

            _lastBytesSent = sent;
            _lastBytesReceived = received;
            _lastCheckTime = now;

            return (Math.Round(upload, 2), Math.Round(download, 2));
        }

        private (long sent, long received) GetCurrentBytes()
        {
            long sent = 0, received = 0;
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;
                var stats = ni.GetIPv4Statistics();
                sent += stats.BytesSent;
                received += stats.BytesReceived;
            }
            return (sent, received);
        }
    }

}
