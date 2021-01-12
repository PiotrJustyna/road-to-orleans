using System;
using System.Globalization;

namespace SiloHost
{
    public static class EnvironmentVariables
    {
        public static string AdvertisedIp(string defaultValue = null) =>
            Environment.GetEnvironmentVariable("ADVERTISEDIP") ?? defaultValue;

        public static string SiloPort => Environment.GetEnvironmentVariable("SILOPORT") ??
                                         throw new Exception("Silo port cannot be null");

        public static string GatewayPort => Environment.GetEnvironmentVariable("GATEWAYPORT") ??
                                            throw new Exception("Gateway port cannot be null");

        public static string AwsRegion => Environment.GetEnvironmentVariable("AWSREGION") ??
                                          throw new Exception("Aws region cannot be null");

        public static string MembershipTable => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                                throw new Exception("Membership table cannot be null");

        public static string DashboardPort => Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                                              throw new Exception("Dashboard port cannot be null");

        public static string EcsContainerMetadataUri() =>
            Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
    }
}