using System;

namespace Api
{
    public class EnvironmentVariables
    {
        public static string AwsRegion => Environment.GetEnvironmentVariable("AWSREGION") ??
                                          throw new Exception("Aws region cannot be null");

        public static string MembershipTable => Environment.GetEnvironmentVariable("MEMBERSHIPTABLE") ??
                                                throw new Exception("Membership table cannot be null");
    }
}