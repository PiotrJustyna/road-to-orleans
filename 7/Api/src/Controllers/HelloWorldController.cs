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
        public async Task<IActionResult> Get(string name, int clientId, CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                if (clientId >= 0 && clientId <= 100)
                {
                    var grainCancellationTokenSource = new GrainCancellationTokenSource();

                    //A random integer is generated to allow for a new hello world grain to be created per client creation.
                    result = Ok(await _clusterClient.GetGrain<IHelloWorld>(_generator.Next(int.MaxValue))
                        .SayHello(name, grainCancellationTokenSource.Token));
                }
                else
                {
                    result = BadRequest($"{nameof(clientId)} not in the expected range (0 - 100).");
                }
            }

            return result;
        }
    }
}