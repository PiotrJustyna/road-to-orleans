using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.src.TRX;

namespace Grains
{
    public class Test2 : Orleans.Grain, ITest2
    {
        public async Task<TestDetails> HelloWorldTest(string testlistId)
        {
            var executionTimeDetails = new UnitTestExecutionTime();
            var stopwatch = new Stopwatch();

            executionTimeDetails.StartTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            stopwatch.Start();
            
            await Task.Delay(200);

            stopwatch.Stop();
            executionTimeDetails.EndTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            executionTimeDetails.Duration = stopwatch.Elapsed.ToString();
            
            var unitTest = Helpers.UnitTestCreator(this.GetType(), Helpers.CallerName(), testlistId, executionTimeDetails);
            
            return unitTest;
        }
    }
}