using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class Test1 : Orleans.Grain, ITest1
    {
        public async Task<bool> HelloWorldTest()
        {
            await Task.Delay(1000);
            return await Task.FromResult(true);
        }
    }
}