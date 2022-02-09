using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace SiloHost.Controllers
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
        public async Task<long> Get(
            int n,
            CancellationToken cancellationToken)
        {
            var grain = _clusterClient.GetGrain<IHelloWorld>(1);
            return await grain.Factorial(n);
        }
    }
}