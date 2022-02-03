using System.Threading.Tasks;
using Interfaces;
using Interfaces.src.TRX;

namespace Grains
{
    public class Test2 : Orleans.Grain, ITest2
    {
        public async Task<TestDetails> HelloWorldTest()
        {
            await Task.Delay(200);
            var unitTest = Helpers.UnitTestCreator(this.GetType(), Helpers.CallerName());
            return unitTest;
        }
    }
}