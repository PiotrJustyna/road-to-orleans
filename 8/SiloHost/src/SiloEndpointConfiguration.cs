using System.Net;

namespace SiloHost
{
    public record SiloEndpointConfiguration
    {
        public IPAddress Ip { get; }
        public int SiloPort { get; }
        public int GatewayPort { get; }
    
        public SiloEndpointConfiguration(
            IPAddress ip,
            int siloPort,
            int gatewayPort) => (Ip, SiloPort, GatewayPort) = (ip, siloPort, gatewayPort);
        
        public SiloEndpointConfiguration(
            string ip,
            int siloPort,
            int gatewayPort)
        {
            if (IPAddress.TryParse(ip, out var parsedIp))
            {
                Ip = parsedIp;
            }
            
            SiloPort = siloPort;
            GatewayPort = gatewayPort;
        }
    }
}