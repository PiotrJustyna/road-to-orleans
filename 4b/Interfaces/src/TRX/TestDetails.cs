namespace Interfaces.src.TRX
{
    public class TestDetails
    {
        public UnitTestResult UnitTestResult { get; set; }
        
        public UnitTestDefinition UnitTestDefinition { get; set; }
        
        public TestEntry TestEntry { get; set; }
        public bool TestOutcome { get; set; }
    }
}