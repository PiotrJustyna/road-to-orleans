using System.Threading.Tasks;

namespace Interfaces
{
    public interface ITest2 : Orleans.IGrainWithIntegerKey
    {
        Task<bool> HelloWorldTest();
    }
}