using System;

namespace Interfaces.src.TRX
{
    public class TestCreatorParameters
    {
        public Type ClassType { get; set; }
        public string CallerName { get; set; }
        public string TestListId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string MachineName { get; set; }
        public bool TestOutcome { get; set; }
    }
}