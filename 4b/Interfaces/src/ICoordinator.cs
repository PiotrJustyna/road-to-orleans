using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICoordinator : Orleans.IGrainWithIntegerKey
    {
        Task<string> RunTests();
    }
}