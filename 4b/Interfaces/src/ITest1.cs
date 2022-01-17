using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITest1 : Orleans.IGrainWithIntegerKey
    {
        Task<bool> HelloWorldTest();
    }
}