using Orleans;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloWorld : IGrainWithIntegerKey
    {
        Task<string> SayHello(string name, GrainCancellationToken grainCancellationToken);
    }
}
