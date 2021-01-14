using System.Collections.Generic;

namespace SiloHost
{
    public record EcsMetadata
    {
        public List<Port> Ports { get; init; }
    }

    public record Port
    {
        public int HostPort { get; init; }

        public int ContainerPort { get; init; }
    }
}