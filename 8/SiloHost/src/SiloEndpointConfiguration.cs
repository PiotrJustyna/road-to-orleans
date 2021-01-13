using System.Net;

namespace SiloHost
{
    public record SiloEndpointConfiguration
    {
        public IPAddress Ip { get; }
        public int SiloPort { get; }
        public int GatewayPort { get; }
        public int DashboardPort { get; }
        public int SiloListeningPort { get; }
        public int GatewayListeningPort { get; }
        
        public SiloEndpointConfiguration(
            IPAddress ip,
            int siloPort,
            int gatewayPort,
            int dashboardPort) => (Ip, SiloPort, GatewayPort, DashboardPort) = (ip, siloPort, gatewayPort, dashboardPort);
    }
}