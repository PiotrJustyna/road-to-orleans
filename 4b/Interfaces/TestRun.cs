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

    [XmlRoot("TestRun", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
    public class TestRun
    {
        [XmlAttribute(AttributeName = "id")] public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] public string Name { get; set; }

        public Times Times { get; set; }

        public TestSettings TestSettings { get; set; }
    }
}