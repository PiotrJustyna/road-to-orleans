using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITimerWorld : IGrainWithIntegerKey
    {
        Task<string> ActivateTimer(GrainCancellationToken grainCancellationToken);
        Task<string> DeactivateGrain(GrainCancellationToken grainCancellationToken);
        Task<string> DeactivateTimer(GrainCancellationToken grainCancellationToken);
    }
}
