using System;
using System.Globalization;

namespace SiloHost
{
    public class EnvironmentVariables:IEnvironmentVariables
    {
        public string AdvertisedIp(string defaultValue = null) =>
            Environment.GetEnvironmentVariable("ADVERTISEDIP") ?? defaultValue;

        public int SiloPort()
        {
            var siloPort = Environment.GetEnvironmentVariable("SILOPORT")??throw new Exception("Silo port cannot be null");
            return int.Parse(siloPort);
        }

        public int GatewayPort()
        {
            var gatewayPort = Environment.GetEnvironmentVariable("GATEWAYPORT") ??
                              throw new Exception("Gateway port cannot be null");
            return int.Parse(gatewayPort);
        }

        public string AwsRegion() => Environment.GetEnvironmentVariable("AWSREGION") ??
                                     throw new Exception("Aws region cannot be null");

        public string MembershipTable() => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                                throw new Exception("Membership table cannot be null");

        public bool IsLocal()
        {
            return !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ISLOCAL")) && bool.Parse(Environment.GetEnvironmentVariable("ISLOCAL"));
        }

        public int DashboardPort()
        {
            var dashboardBoardPort = Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                   throw new Exception("Dashboard port cannot be null");
            return int.Parse(dashboardBoardPort);
        }

        public string EcsContainerMetadataUri() =>
            Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
    }

    public interface IEnvironmentVariables
    {
        string AdvertisedIp(string defaultValue = null);
        int SiloPort();
        int GatewayPort();
        string AwsRegion();
        string MembershipTable();
        int DashboardPort();
        string EcsContainerMetadataUri();
        bool IsLocal();
    }
}