using System;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.src.TRX;

namespace Grains
{
    public class Test1 : Orleans.Grain, ITest1
    {
        public async Task<TestDetails> HelloWorldTest(string testListId)
        {
            await Task.Delay(100);
            var unitTest = Helpers.UnitTestCreator(this.GetType(), Helpers.CallerName(), testListId);
            return unitTest;
        }
    }
}