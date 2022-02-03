using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
    [XmlRoot(ElementName = "TestList")]
    public class TestList
    {
        [XmlAttribute(AttributeName = "id")] 
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")] 
        public string Name { get; set; }
    }
}