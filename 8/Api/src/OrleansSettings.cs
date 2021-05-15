using System;

namespace Api
{
    public class OrleansSettings : IOrleansSettings
    {
        public string AwsRegion { get; set; }

        public string MembershipTable { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(AwsRegion) || string.IsNullOrEmpty(MembershipTable))
            {
                throw new InvalidOperationException($"Orleans settings are missing: {AwsRegion} - {MembershipTable}");
            }
        }
    }

    public interface IOrleansSettings
    {
        string AwsRegion { get; set; }
        string MembershipTable { get; set; }

        void Validate();
    }
}