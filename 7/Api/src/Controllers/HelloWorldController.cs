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

        public HelloWorldController(
            IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string name, int clientId, CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                if (clientId >= 0 && clientId <= 100)
                {
                    var grainCancellationTokenSource = new GrainCancellationTokenSource();
                    //A set integer(1 - 100) is accepted to allow for a new hello world grain to be created or reused per client.
                      result = Ok(await _clusterClient.GetGrain<IHelloWorld>(clientId)
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