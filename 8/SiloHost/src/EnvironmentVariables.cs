using System;

namespace SiloHost
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        public string GetAdvertisedIp(string defaultValue = null) =>
            Environment.GetEnvironmentVariable("ADVERTISEDIP") ?? defaultValue;

        public int GetSiloPort()
        {
            var siloPort = Environment.GetEnvironmentVariable("SILOPORT") ??
                           throw new Exception("Silo port cannot be null");
            return int.Parse(siloPort);
        }

        public int GetGatewayPort()
        {
            var gatewayPort = Environment.GetEnvironmentVariable("GATEWAYPORT") ??
                              throw new Exception("Gateway port cannot be null");
            return int.Parse(gatewayPort);
        }
        
        public int GetDashboardPort()
        {
            var dashboardBoardPort = Environment.GetEnvironmentVariable("DASHBOARDPORT") ??
                                     throw new Exception("Dashboard port cannot be null");
            return int.Parse(dashboardBoardPort);
        }

        public string GetAwsRegion() => Environment.GetEnvironmentVariable("AWSREGION") ??
                                     throw new Exception("Aws region cannot be null");

        public string GetMembershipTableName() => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                           throw new Exception("Membership table cannot be null");

        public bool GetIsLocal()
        {
            var isLocal = Environment.GetEnvironmentVariable("ISLOCAL") ??
                          throw new Exception(
                              "ISLOCAL env variable is needed to determine if the cluster is running locally or in the cloud.");
            return bool.Parse(isLocal);
        }

        public string GetEcsContainerMetadataUri() =>
            Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
        public string GetClusterId() =>
            Environment.GetEnvironmentVariable("CLUSTER_ID") ?? throw new Exception("Cluster Id cannot be null");
    }

    public interface IEnvironmentVariables
    {
        string GetAdvertisedIp(string defaultValue = null);
        int GetSiloPort();
        int GetGatewayPort();
        string GetAwsRegion();
        string GetMembershipTableName();
        int GetDashboardPort();
        string GetEcsContainerMetadataUri();
        bool GetIsLocal();
        string GetClusterId();
    }
}