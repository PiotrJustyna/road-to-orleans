using System;

namespace Grains
{
    [Serializable]
    public class HelloWorldState
    {
        public DateTime GreetingTimeUtc { get; set; }
    }
}