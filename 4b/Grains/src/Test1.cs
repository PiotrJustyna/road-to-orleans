using System;
using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class Test1 : Orleans.Grain, ITest1
    {
        public async Task<UnitTestDefinition> HelloWorldTest()
        {
            await Task.Delay(1000);

            var methodName = Helpers.CallerName();
            var classFullName = this.GetType().FullName;
            var assemblyName = this.GetType().Assembly.GetName();
            
            var testDefinition = new UnitTestDefinition()
            {
                Id = Guid.NewGuid().ToString(),
                Execution = new Execution()
                {
                    Id = Guid.NewGuid().ToString()
                },
                Name = $"{classFullName}.{methodName}",
                Storage = $"{assemblyName.Name}.dll",
                TestMethod = new TestMethod()
                {
                    AdapterTypeName = "orleans",
                    ClassName = classFullName,
                    Name = methodName,
                    CodeBase = $"{assemblyName.Name}.dll"
                }
            };
            return testDefinition;
        }
    }
}