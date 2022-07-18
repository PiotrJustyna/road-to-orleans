using Orleans;

namespace Interfaces
{
    public interface IHello : IGrainWithIntegerKey
    {
        Task<string> PrintHello(string greeting);
        Task<string> TraceHello(int milliSeconds);
        Task MetricHello(int iterations);
    }
}