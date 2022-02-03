using System;
using System.Runtime.CompilerServices;
using Interfaces.src.TRX;

namespace Interfaces
{
    public static class Helpers
    {
        public static string CallerName([CallerMemberName]string name = "")
        {
            return name;
        }

        public static TestDetails UnitTestCreator(Type classType, string callerName, string testListId)
        {
            var testId = Guid.NewGuid().ToString();
            var executionId = Guid.NewGuid().ToString();
            
            var methodName = callerName;
            var classFullName = classType.FullName;
            var assemblyName = $"{classType.Assembly.GetName().Name}.dll";
            
            var unitTestResult = new UnitTestResult()
            {
                ExecutionId = executionId,
                TestId = testId,
                TestName = "Placeholder",
                ComputerName = "Placeholder",
                Duration = "Placeholder",
                StartTime = "Placeholder",
                EndTime = "Placeholder",
                TestType = "Placeholder",
                Outcome = "Placeholder",
                TestListId = testListId,
                RelativeResultsDirectory = executionId
            };
            
            var unitTestDefinition = new UnitTestDefinition()
            {
                Id = testId,
                Execution = new Execution()
                {
                    Id = executionId
                },
                Name = $"{classFullName}.{methodName}",
                Storage = assemblyName,
                TestMethod = new TestMethod()
                {
                    AdapterTypeName = "orleans",
                    ClassName = classFullName,
                    Name = methodName,
                    CodeBase = assemblyName
                }
            };

            var testEntry = new TestEntry()
            {
                TestId = testId,
                ExecutionId = executionId,
                TestListId = testListId
            };

            return new TestDetails()
            {
                UnitTestDefinition = unitTestDefinition,
                UnitTestResult = unitTestResult,
                TestEntry = testEntry
            };
        }
    }
}