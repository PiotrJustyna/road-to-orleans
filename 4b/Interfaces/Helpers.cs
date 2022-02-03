using System;
using System.Runtime.CompilerServices;
using Interfaces.src.TRX;

namespace Interfaces
{
    public static class Helpers
    {
        public static string CallerName([CallerMemberName]string name = "")
        {
            return name;
        }

        public static TestDetails UnitTestCreator(Type classType, string callerName )
        {
            var methodName = callerName;
            var classFullName = classType.FullName;
            var assemblyName = classType.Assembly.GetName();
            
            var unitTest = new UnitTestDefinition()
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

            var unitTestResult = new UnitTestResult()
            {

            };

            return new TestDetails()
            {
                UnitTestDefinition = unitTest,
                UnitTestResult = unitTestResult
            };
        }
    }
}