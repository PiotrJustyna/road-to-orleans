using System.Collections.Generic;

namespace SiloHost
{
    public class EcsMetadata
    {
        public List<Port> Ports { get; set; }
    }

    public class Port
    {
        public int HostPort { get; set; }

        public int ContainerPort { get; set; }
    }
}