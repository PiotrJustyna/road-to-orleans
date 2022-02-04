using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
    [XmlRoot(ElementName = "TestEntry")]
    public class TestEntry
    {
        [XmlAttribute(AttributeName = "testId")]
        public string TestId { get; set; }

        [XmlAttribute(AttributeName = "executionId")]
        public string ExecutionId { get; set; }

        [XmlAttribute(AttributeName = "testListId")]
        public string TestListId { get; set; }
    }    
}