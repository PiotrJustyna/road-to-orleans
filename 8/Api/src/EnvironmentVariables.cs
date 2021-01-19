using System;

namespace Api
{
    public class EnvironmentVariables:IEnvironmentVariables
    {
        public string GetAwsRegion() => Environment.GetEnvironmentVariable("AWSREGION") ??
                                          throw new Exception("Aws region cannot be null");

        public string GetMembershipTable() => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                                throw new Exception("Membership table cannot be null");
        public string GetClusterId() =>
            Environment.GetEnvironmentVariable("CLUSTER_ID") ?? throw new Exception("Cluster Id cannot be null");
    }

    public interface IEnvironmentVariables
    {
        string GetAwsRegion();
        string GetMembershipTable();
        string GetClusterId();
    }
}