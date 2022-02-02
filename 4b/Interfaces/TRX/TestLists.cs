using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Interfaces.TRX
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