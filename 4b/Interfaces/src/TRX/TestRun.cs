using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
    [XmlRoot("TestRun", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public class TestRun
    {
        [XmlAttribute(AttributeName = "id")] 
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] 
        public string Name { get; set; }

        public Times Times { get; set; }

        public TestSettings TestSettings { get; set; }

        public Results TestResults { get; set; }

        public TestDefinitions TestDefinitions { get; set; }

        public List<TestEntry> TestEntries { get; set; }

        public List<TestList> TestLists { get; set; }

        public ResultSummary ResultSummary { get; set; }
    }

    [XmlRoot(ElementName = "Times")]
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
        [XmlAttribute(AttributeName = "id")] 
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] 
        public string Name { get; set; }

        [Obsolete]
        public Deployment Deployment { get; set; }
    }

    public class Deployment
    {
        [XmlAttribute(AttributeName = "runDeploymentRoot")]
        public string RunDeploymentRoot { get; set; }
    }
    
    public class ResultSummary
    {
        [XmlElement(ElementName = "Counters")]
        public Counters Counters { get; set; }

        [XmlElement(ElementName = "Output")]
        public Output Output { get; set; }

        [XmlAttribute(AttributeName = "outcome")]
        public string Outcome { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Counters")]
    public class Counters
    {

        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }

        [XmlAttribute(AttributeName = "executed")]
        public string Executed { get; set; }

        [XmlAttribute(AttributeName = "passed")]
        public string Passed { get; set; }

        [XmlAttribute(AttributeName = "failed")]
        public string Failed { get; set; }

        [XmlAttribute(AttributeName = "error")]
        public string Error { get; set; }

        [XmlAttribute(AttributeName = "timeout")]
        public string Timeout { get; set; }

        [XmlAttribute(AttributeName = "aborted")]
        public string Aborted { get; set; }

        [XmlAttribute(AttributeName = "inconclusive")]
        public string Inconclusive { get; set; }

        [XmlAttribute(AttributeName = "passedButRunAborted")]
        public string PassedButRunAborted { get; set; }

        [XmlAttribute(AttributeName = "notRunnable")]
        public string NotRunnable { get; set; }

        [XmlAttribute(AttributeName = "notExecuted")]
        public string NotExecuted { get; set; }

        [XmlAttribute(AttributeName = "disconnected")]
        public string Disconnected { get; set; }

        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }

        [XmlAttribute(AttributeName = "completed")]
        public string Completed { get; set; }

        [XmlAttribute(AttributeName = "inProgress")]
        public string InProgress { get; set; }

        [XmlAttribute(AttributeName = "pending")]
        public string Pending { get; set; }
    }

    [XmlRoot(ElementName = "Output")]
    public class Output
    {

        [XmlElement(ElementName = "StdOut")]
        public string StdOut { get; set; }
    }
}