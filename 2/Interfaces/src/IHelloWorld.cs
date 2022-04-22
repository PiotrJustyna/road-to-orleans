using System.Threading.Tasks;

namespace Interfaces
{
    public interface IHelloWorld : Orleans.IGrainWithIntegerKey
    {
        ValueTask<string> SayHello(string name);
    }
}
