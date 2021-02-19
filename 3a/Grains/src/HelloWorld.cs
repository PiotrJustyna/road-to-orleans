using System;
using System.Threading.Tasks;
using Interfaces;
using Orleans.Runtime;

namespace Grains
{
    public class HelloWorld : Orleans.Grain, IHelloWorld
    {
        private readonly IPersistentState<HelloWorldState> _helloWorldState;

        public HelloWorld([PersistentState("helloWorldState", "helloWorldStore")]
            IPersistentState<HelloWorldState> helloWorldState)
        {
            _helloWorldState = helloWorldState;
        }

        public async Task<string> SayHello(string name)
        {
            var now = DateTime.UtcNow;

            _helloWorldState.State.GreetingTimeUtc = now;

            await _helloWorldState.WriteStateAsync();

            return $@"Hello {name}! Sent at {now}.";
        }
    }
}