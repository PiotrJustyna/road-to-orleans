using System.Threading;
using System.Threading.Tasks;
using Orleans;

namespace Interfaces
{
    public interface IHelloWorld : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string name);

        Task SayHelloFireAndForget(
            string name,
            GrainCancellationToken cancellationToken);
    }
}
