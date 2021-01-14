using System;
using System.Globalization;

namespace SiloHost
{
    public class EnvironmentVariables:IEnvironmentVariables
    {
        public string AdvertisedIp(string defaultValue = null) =>
            Environment.GetEnvironmentVariable("ADVERTISEDIP") ?? defaultValue;

        public string SiloPort() => Environment.GetEnvironmentVariable("SILOPORT") ??
                                         throw new Exception("Silo port cannot be null");

        public string GatewayPort() => Environment.GetEnvironmentVariable("GATEWAYPORT") ??
                                            throw new Exception("Gateway port cannot be null");

        public string AwsRegion() => Environment.GetEnvironmentVariable("AWSREGION") ??
                                          throw new Exception("Aws region cannot be null");

        public string MembershipTable() => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                                throw new Exception("Membership table cannot be null");

        public string DashboardPort() => Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                                              throw new Exception("Dashboard port cannot be null");

        public string EcsContainerMetadataUri() =>
            Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
    }

    public interface IEnvironmentVariables
    {
        string AdvertisedIp(string defaultValue = null);
        string SiloPort();
        string GatewayPort();
        string AwsRegion();
        string MembershipTable();
        string DashboardPort();
        string EcsContainerMetadataUri();
    }
}