using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Interfaces;

namespace Grains
{
    public class Test2 : Orleans.Grain, ITest2
    {
        public async Task<UnitTestDefinition> HelloWorldTest()
        {
            await Task.Delay(2000);
            
            var methodName = CallerName();
            var classFullName = this.GetType().FullName;
            var assembly = Assembly.GetAssembly(typeof(Test1));
            
            var testDefinition = new UnitTestDefinition()
            {
                Id = Guid.NewGuid().ToString(),
                Execution = new Execution()
                {
                    Id = Guid.NewGuid().ToString()
                },
                Name = $"{classFullName}.{methodName}",
                Storage = assembly.FullName,
                TestMethod = new TestMethod()
                {
                    AdapterTypeName = "Orleans",
                    ClassName = classFullName,
                    Name = methodName,
                    CodeBase = $"/{assembly.Location}/"
                }
            };
            return testDefinition;
        }
        
        private static string CallerName([CallerMemberName]string name = "")
        {
            return name;
        }
    }
}