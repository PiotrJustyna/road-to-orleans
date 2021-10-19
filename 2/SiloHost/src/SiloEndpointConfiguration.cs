using System.Net;

namespace SiloHost
{
    public record SiloEndpointConfiguration(IPAddress Ip, int SiloPort, int GatewayPort);
}