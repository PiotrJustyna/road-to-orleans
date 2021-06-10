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
    }
}