using System.Threading.Tasks;
using Interfaces.src.TRX;

namespace Interfaces
{
    public interface ITest1 : Orleans.IGrainWithIntegerKey
    {
        Task<TestDetails> HelloWorldTest();
    }
}