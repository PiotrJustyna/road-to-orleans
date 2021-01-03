using System.Threading.Tasks;
using Orleans;

namespace Interfaces
{
    public interface IHelloWorld : IGrainWithIntegerKey
    {
        Task<string> SayHello(string name, GrainCancellationToken grainCancellationToken);
    }
}
