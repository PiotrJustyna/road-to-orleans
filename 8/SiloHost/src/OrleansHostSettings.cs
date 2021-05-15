using System;

namespace SiloHost
{
    public class OrleansHostSettings : IOrleansHostSettings
    {
        public string AdvertisedIp { get; set; }
        public int SiloPort { get; set; }
        public int GatewayPort { get; set; }
        public int DashboardPort { get; set; }
        public string AwsRegion { get; set; }
        public string MembershipTableName { get; set; }
        public bool? IsLocal { get; set; }
        public string EcsContainerMetadataUri { get; set; }

        public void Validate()
        {
            if (SiloPort < 1 || GatewayPort < 1 || DashboardPort < 1 || 
                string.IsNullOrEmpty(AwsRegion) || string.IsNullOrEmpty(MembershipTableName) || !IsLocal.HasValue)
            {
                throw new InvalidOperationException($"Orleans settings are missing: {SiloPort} - {GatewayPort} - " +
                    $"{DashboardPort} - {AwsRegion} - {MembershipTableName} - {IsLocal}");
            }
        }
    }

    public interface IOrleansHostSettings
    {
        string AdvertisedIp { get; set; }
        int SiloPort { get; set; }
        int GatewayPort { get; set; }
        string AwsRegion { get; set; }
        string MembershipTableName { get; set; }
        int DashboardPort { get; set; }
        bool? IsLocal { get; set; }
        string EcsContainerMetadataUri { get; set; }

        void Validate();
    }
}