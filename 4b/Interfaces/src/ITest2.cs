using System.Threading.Tasks;
using Interfaces.TRX;

namespace Interfaces
{
    public interface ITest2 : Orleans.IGrainWithIntegerKey
    {
        Task<UnitTest> HelloWorldTest();
    }
}