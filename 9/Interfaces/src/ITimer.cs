using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITimer : IGrainWithIntegerKey
    {
        Task ActivateTimer(GrainCancellationToken grainCancellationToken);
        Task DeactivateGrain(GrainCancellationToken grainCancellationToken);
        Task DeactivateTimer(GrainCancellationToken grainCancellationToken);
    }
}
