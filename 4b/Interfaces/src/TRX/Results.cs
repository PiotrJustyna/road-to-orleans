using System.Collections.Generic;
using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
    [XmlRoot(ElementName = "Results")]
    public class Results
    {
        [XmlElement(ElementName = "UnitTestResult")]
        public List<UnitTestResult> UnitTestResults { get; set; }
    }
    
    [XmlRoot(ElementName = "UnitTestResult")]
    public class UnitTestResult
    {
        [XmlAttribute(AttributeName = "executionId")]
        public string ExecutionId { get; set; }

        [XmlAttribute(AttributeName = "testId")]
        public string TestId { get; set; }

        [XmlAttribute(AttributeName = "testName")]
        public string TestName { get; set; }

        [XmlAttribute(AttributeName = "computerName")]
        public string ComputerName { get; set; }

        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }

        [XmlAttribute(AttributeName = "startTime")]
        public string StartTime { get; set; }

        [XmlAttribute(AttributeName = "endTime")]
        public string EndTime { get; set; }

        [XmlAttribute(AttributeName = "testType")]
        public string TestType { get; set; }

        [XmlAttribute(AttributeName = "outcome")]
        public string Outcome { get; set; }

        [XmlAttribute(AttributeName = "testListId")]
        public string TestListId { get; set; }

        [XmlAttribute(AttributeName = "relativeResultsDirectory")]
        public string RelativeResultsDirectory { get; set; }
    }
}