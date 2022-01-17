using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class Test2 : Orleans.Grain, ITest2
    {
        public async Task<bool> HelloWorldTest()
        {
            await Task.Delay(2000);
            
            return await Task.FromResult(true);
        }
    }
}