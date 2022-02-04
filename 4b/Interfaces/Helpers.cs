using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.Serialization;
using Interfaces.src.TRX;

namespace Interfaces
{
    public static class Helpers
    {
        public static string CallerName([CallerMemberName]string name = "")
        {
            return name;
        }

        public static TestDetails UnitTestCreator(TestCreatorParameters parameters)
        {
            var testId = Guid.NewGuid().ToString();
            var executionId = Guid.NewGuid().ToString();

            var classFullName = parameters.ClassType.FullName;
            var testName = $"{classFullName}.{parameters.CallerName}";
            var assemblyName = $"{parameters.ClassType.Assembly.GetName().Name}.dll";
            
            var unitTestResult = new UnitTestResult()
            {
                ExecutionId = executionId,
                TestId = testId,
                TestName = testName,
                ComputerName = parameters.MachineName,
                Duration = parameters.TestExecutionTime.Duration,
                StartTime = parameters.TestExecutionTime.StartTime,
                EndTime = parameters.TestExecutionTime.EndTime,
                TestType = "TestTypePlaceholder",
                TestListId = parameters.TestListId,
                RelativeResultsDirectory = executionId,
                Outcome = "Passed"
            };
            
            var unitTestDefinition = new UnitTestDefinition()
            {
                Id = testId,
                Execution = new Execution()
                {
                    Id = executionId
                },
                Name = testName,
                Storage = assemblyName,
                TestMethod = new TestMethod()
                {
                    AdapterTypeName = "orleans",
                    ClassName = classFullName,
                    Name = parameters.CallerName,
                    CodeBase = assemblyName
                }
            };

            var testEntry = new TestEntry()
            {
                TestId = testId,
                ExecutionId = executionId,
                TestListId = parameters.TestListId
            };

            return new TestDetails()
            {
                UnitTestDefinition = unitTestDefinition,
                UnitTestResult = unitTestResult,
                TestEntry = testEntry
            };
        }

        public static XDocument TrxDocumentCreator(TestRun testRun)
        {
            var serializer = new XmlSerializer(typeof(TestRun));
            var serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add(
                prefix: "",
                ns: "");
            
            var writer = new StringWriter();
            serializer.Serialize(
                writer,
                testRun,
                serializerNamespaces);

            var document = XDocument.Parse(writer.ToString());
            document.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
            return document;
        }

        public static ResultSummary ResultSummaryCreator(List<bool> testResultsOutcome, string outcome)
        {
            var resultSummary = new ResultSummary()
            {
                Outcome = outcome,
                Output = new Output()
                {
                    StdOut = "Orleans output"
                },
                Counters = new Counters()
                {
                    Aborted = "0",
                    Completed = testResultsOutcome.Count(tro => tro).ToString(),
                    Executed = testResultsOutcome.Count().ToString(),
                    Disconnected = "0",
                    Error = testResultsOutcome.Count(tro => !tro).ToString(),
                    Failed = testResultsOutcome.Count(tro => !tro).ToString(),
                    Inconclusive = "0",
                    InProgress = "0",
                    NotRunnable = "0",
                    NotExecuted = "0"
                }
            };
            return resultSummary;
        }
    }
}