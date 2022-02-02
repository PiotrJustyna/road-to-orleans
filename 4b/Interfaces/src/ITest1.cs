using System.Threading.Tasks;
using Interfaces.TRX;

namespace Interfaces
{
    public interface ITest1 : Orleans.IGrainWithIntegerKey
    {
        Task<UnitTest> HelloWorldTest();
    }
}