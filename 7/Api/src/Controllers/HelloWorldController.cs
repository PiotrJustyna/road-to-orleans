using System;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
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
        public async Task<IActionResult> Get(string name, CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();
                //A random integer (1 - 50) is generated to allow for a new hello world grain to be created or reused per client.
                var response = await _clusterClient.GetGrain<IHelloWorld>(_generator.Next(1, 51))
                    .SayHello(name, grainCancellationTokenSource.Token);

                result = string.IsNullOrEmpty(response)
                    ? NoContent()
                    : Ok(response);

            }

            return result;
        }
    }
}