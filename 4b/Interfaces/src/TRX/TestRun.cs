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
}