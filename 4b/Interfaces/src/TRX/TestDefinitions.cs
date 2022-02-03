using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Interfaces.src.TRX
{
    [XmlRoot(ElementName = "TestDefinitions")]
    public class TestDefinitions
    {
        [XmlElement(ElementName = "UnitTest")]
        public List<UnitTestDefinition> UnitTests { get; set; }
    }

    [XmlRoot(ElementName = "UnitTest")]
    public class UnitTestDefinition
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "storage")]
        public string Storage { get; set; }

        [XmlElement(ElementName = "Execution")]
        public Execution Execution { get; set; }

        [XmlElement(ElementName = "TestMethod")]
        public TestMethod TestMethod { get; set; }
    }

    [XmlRoot(ElementName = "Execution")]
    public class Execution
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "TestMethod")]
    public class TestMethod
    {
        [XmlAttribute(AttributeName = "codeBase")]
        public string CodeBase { get; set; }

        [XmlAttribute(AttributeName = "className")]
        public string ClassName { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        
        [XmlAttribute(AttributeName = "adapterTypeName")]
        public string AdapterTypeName { get; set; }
    }
}