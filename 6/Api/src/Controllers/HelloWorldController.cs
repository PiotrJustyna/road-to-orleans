using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly Random _generator;

        public HelloWorldController(
            IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _generator = new Random();
        }

        [HttpGet]
        public async Task<string> Get(string name, CancellationToken cancellationToken)
        {
            string result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();

                //A random integer is generated to allow for a new hello world grain to be created per client creation.
                result = await _clusterClient.GetGrain<IHelloWorld>(_generator.Next(int.MaxValue))
                    .SayHello(name, grainCancellationTokenSource.Token);

            }

            return result;
        }
    }
}
