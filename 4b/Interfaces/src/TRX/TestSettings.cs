using System;
using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
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
}