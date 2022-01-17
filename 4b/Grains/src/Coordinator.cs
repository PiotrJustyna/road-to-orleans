using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class Coordinator : Orleans.Grain, ICoordinator
    {
        public async Task<string> RunTests()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var tests = new List<Task>
            {
                GrainFactory.GetGrain<ITest1>(1).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(2).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(3).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(4).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(5).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(6).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(7).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(8).HelloWorldTest()
            };

            await Task.WhenAll(tests);
            
            stopwatch.Stop();

            return await Task.FromResult($"All done! Elapsed time: {stopwatch.ElapsedMilliseconds}ms.");
        }
    }
}