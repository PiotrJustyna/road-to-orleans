using System.Collections.Generic;
using System.Xml.Serialization;

namespace Interfaces
{
    public class Times
    {
        [XmlAttribute(AttributeName = "creation")]
        public string Creation { get; set; }

        [XmlAttribute(AttributeName = "queuing")]
        public string Queuing { get; set; }

        [XmlAttribute(AttributeName = "start")]
        public string Start { get; set; }

        [XmlAttribute(AttributeName = "finish")]
        public string Finish { get; set; }
    }

    public class TestSettings
    {
        [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

        public Deployment Deployment { get; set; }
    }

    public class Deployment
    {
        [XmlAttribute(AttributeName = "runDeploymentRoot")]
        public string RunDeploymentRoot { get; set; }
    }

    // 2022-01-18 PJ:
    // TODO
    public class Results
    {
    }

    // 2022-01-18 PJ:
    // TODO
    public class TestDefinitions
    {
    }

    // 2022-01-18 PJ:
    // TODO
    public class ResultSummary
    {
        [XmlAttribute(AttributeName = "outcome")] public string Outcome { get; set; }
    }

    // 2022-01-18 PJ:
    // TODO
    public class TestEntry
    {
        [XmlAttribute(AttributeName = "testId")]
        public string TestId { get; set; }

        [XmlAttribute(AttributeName = "executionId")]
        public string ExecutionId { get; set; }

        [XmlAttribute(AttributeName = "testListId")]
        public string TestListId { get; set; }
    }

    public class TestList
    {
        [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] public string Name { get; set; }
    }

    [XmlRoot("TestRun", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public class TestRun
    {
        [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

        public Times Times { get; set; }

        public TestSettings TestSettings { get; set; }

        public Results TestResults { get; set; }

        public TestDefinitions TestDefinitions { get; set; }

        public List<TestEntry> TestEntries { get; set; }

        public List<TestList> TestLists { get; set; }
        
        public ResultSummary ResultSummary { get; set; }
    }
}