using System;
using System.Threading.Tasks;
using Interfaces;
using Interfaces.TRX;

namespace Grains
{
    public class Test1 : Orleans.Grain, ITest1
    {
        public async Task<UnitTest> HelloWorldTest()
        {
            await Task.Delay(1000);

            var methodName = Helpers.CallerName();
            var classFullName = this.GetType().FullName;
            var assemblyName = this.GetType().Assembly.GetName();
            
            var unitTest = new UnitTest()
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
            return unitTest;
        }
    }
}