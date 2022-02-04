using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.src.TRX;

namespace Grains
{
    public class Test1 : Orleans.Grain, ITest1
    {
        public async Task<TestDetails> HelloWorldTest(string testListId)
        {
            var executionTimeDetails = new UnitTestExecutionTime();
            var stopwatch = new Stopwatch();

            executionTimeDetails.StartTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            stopwatch.Start();
            
            await Task.Delay(100);

            stopwatch.Stop();
            executionTimeDetails.EndTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            executionTimeDetails.Duration = stopwatch.Elapsed.ToString();
            
            var unitTest = Helpers.UnitTestCreator(this.GetType(), Helpers.CallerName(), testListId, executionTimeDetails);

            return unitTest;
        }
    }
}