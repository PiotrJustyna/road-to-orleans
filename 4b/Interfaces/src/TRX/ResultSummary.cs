using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
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