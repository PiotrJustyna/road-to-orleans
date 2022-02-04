using System;
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

        public static TestDetails UnitTestCreator(Type classType, string callerName, string testListId, UnitTestExecutionTime testExecutionTime)
        {
            var testId = Guid.NewGuid().ToString();
            var executionId = Guid.NewGuid().ToString();

            var classFullName = classType.FullName;
            var testName = $"{classFullName}.{callerName}";
            var assemblyName = $"{classType.Assembly.GetName().Name}.dll";
            
            var unitTestResult = new UnitTestResult()
            {
                ExecutionId = executionId,
                TestId = testId,
                TestName = testName,
                ComputerName = Environment.MachineName,
                Duration = testExecutionTime.Duration,
                StartTime =testExecutionTime.StartTime,
                EndTime = testExecutionTime.EndTime,
                TestType = "TestTypePlaceholder",
                TestListId = testListId,
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
                    Name = callerName,
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
    }
}