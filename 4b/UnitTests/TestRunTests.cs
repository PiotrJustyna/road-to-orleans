using System;
using System.IO;
using System.Xml.Serialization;
using Interfaces;
using Xunit;

namespace UnitTests
{
    public class TestRunTests
    {
        private Func<string> GenerateString = () => Guid.NewGuid().ToString();

        [Fact]
        public void TestRunSerialization()
        {
            var serializer = new XmlSerializer(typeof(TestRun));

            var serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add(
                prefix: "",
                ns: "");

            var sampleTestRun = new TestRun
            {
                Id = GenerateString(),
                Name = GenerateString(),
                Times = new Times
                {
                    Creation = GenerateString(),
                    Queuing = GenerateString(),
                    Start = GenerateString(),
                    Finish = GenerateString()
                },
                TestSettings = new TestSettings
                {
                    Id = GenerateString(),
                    Name = GenerateString(),
                    Deployment = new Deployment
                    {
                        RunDeploymentRoot = GenerateString()
                    }
                }
            };

            var writer = new StringWriter();
            serializer.Serialize(
                writer,
                sampleTestRun,
                serializerNamespaces);

            var content = writer.ToString();

            Assert.NotNull(content);
        }
    }
}