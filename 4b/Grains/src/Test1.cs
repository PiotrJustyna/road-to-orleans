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
            var startTime = DateTime.UtcNow;
            await Task.Delay(200);
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;

            var unitTest = Helpers.TestDetailsCreator(new TestCreatorParameters()
            {
                ClassType = GetType(),
                CallerName = Helpers.CallerName(),
                TestListId = testListId,
                StartTime = startTime.ToString("yyyy-MM-ddThh:mm:ss.fff"),
                EndTime = endTime.ToString("yyyy-MM-ddThh:mm:ss.fff"),
                Duration = duration.ToString(),
                MachineName = Environment.MachineName,
                TestOutcome = true
            });

            return unitTest;
        }
    }
}