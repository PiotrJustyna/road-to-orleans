using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Interfaces;
using Interfaces.src.TRX;

namespace Grains
{
    public class Coordinator : Orleans.Grain, ICoordinator
    {
        public async Task<string> RunTests()
        {
            var testListId = Guid.NewGuid().ToString();
            
            var testRun = new TestRun()
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"{GetType().Name}/{MethodBase.GetCurrentMethod().Name}",
                Times = new Times()
                {
                    Creation = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                },
                TestSettings = new TestSettings()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "default"
                },
                TestResults = new Results() { UnitTestResults = new List<UnitTestResult>() },
                TestLists = new List<TestList>()
                {
                    new TestList()
                    {
                        Name = "Results Not in a List",
                        Id = testListId
                    }
                },
                TestDefinitions = new TestDefinitions() { UnitTests = new List<UnitTestDefinition>() }
            };

            testRun.Times.Start = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            testRun.Times.Queuing = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            var tests = new List<Task<TestDetails>>
            {
                GrainFactory.GetGrain<ITest1>(1).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest2>(2).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest1>(3).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest2>(4).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest1>(5).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest2>(6).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest1>(7).HelloWorldTest(testListId),
                GrainFactory.GetGrain<ITest2>(8).HelloWorldTest(testListId)
            };

            await Task.WhenAll(tests);
            testRun.TestResults.UnitTestResults = tests.Select(ts => ts.Result.UnitTestResult).ToList();
            testRun.TestDefinitions.UnitTests = tests.Select(ts => ts.Result.UnitTestDefinition).ToList();
            testRun.TestEntries = tests.Select(ts => ts.Result.TestEntry).ToList();

            var testOutcomeList = tests.Select(ts => ts.Result.TestOutcome).ToList();

            testRun.Times.Finish = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            testRun.ResultSummary = Helpers.ResultSummaryCreator(testOutcomeList, "Completed");

            var trxDocument = Helpers.TrxDocumentCreator(testRun);

            return await Task.FromResult(trxDocument.ToString());
        }
    }
}