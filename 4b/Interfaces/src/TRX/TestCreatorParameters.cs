using System;

namespace Interfaces.src.TRX
{
    public class TestCreatorParameters
    {
        public Type ClassType { get; set; }
        public string CallerName { get; set; }
        public string TestListId { get; set; }
        public UnitTestExecutionTime TestExecutionTime { get; set; }
        public string MachineName { get; set; }
        public bool TestOutcome { get; set; }
    }
}