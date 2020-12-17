using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<HelloWorldController> _logger;
        private readonly IClusterClient _clusterClient;
        private readonly Random _generator;

        public HelloWorldController(
            ILogger<HelloWorldController> logger,
            IClusterClient clusterClient)
        {
            _logger = logger;
            _clusterClient = clusterClient;
            _generator = new Random();
        }

        [HttpGet]
        public async Task<string> Get(string name, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Request {name} received.");

            string result = null;

            var grainCancellationTokenSource = new GrainCancellationTokenSource();

            if (!cancellationToken.IsCancellationRequested)
            {
                //A random integer is generated to allow for a new hello world grain to be created per client creation.
                result = await _clusterClient.GetGrain<IHelloWorld>(_generator.Next(int.MaxValue))
                    .SayHello(name, grainCancellationTokenSource.Token);

            }

            return result;
        }
    }
}
