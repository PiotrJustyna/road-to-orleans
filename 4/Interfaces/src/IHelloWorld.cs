using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloWorld : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string name);
    }
}
