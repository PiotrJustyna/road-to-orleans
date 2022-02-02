using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Interfaces;

namespace Grains
{
    public class Coordinator : Orleans.Grain, ICoordinator
    {
        public async Task<string> RunTests()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var testRun = new TestRun()
            {
                Id = Guid.NewGuid().ToString(),
                Name = $"{this.GetType().Name}/{MethodBase.GetCurrentMethod()?.Name}",
                Times = new Times()
                {
                    Creation = DateTime.Now.ToString(CultureInfo.CurrentCulture)
                }
            };

            var tests = new List<Task>
            {
                GrainFactory.GetGrain<ITest1>(1).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(2).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(3).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(4).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(5).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(6).HelloWorldTest(),
                GrainFactory.GetGrain<ITest1>(7).HelloWorldTest(),
                GrainFactory.GetGrain<ITest2>(8).HelloWorldTest()
            };

            testRun.Times.Start = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            await Task.WhenAll(tests);
            testRun.Times.Finish = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            
            stopwatch.Stop();
            
            var serializer = new XmlSerializer(typeof(TestRun));
            var serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add(
                prefix: "",
                ns: "");
            
            var writer = new StringWriter();
            serializer.Serialize(
                writer,
                testRun,
                serializerNamespaces);

            var document = XDocument.Parse(writer.ToString());
            document.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();

            return await Task.FromResult($"-- Test Execution Details --- \n {document}");
        }
    }
}