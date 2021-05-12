using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimerController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public TimerController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateTimer(CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();

                await _clusterClient.GetGrain<ITimer>(1)
                    .ActivateTimer(grainCancellationTokenSource.Token);

                result = Ok("Timer activated");
            }

            return result;
        }

        [HttpGet("deactivate-timer")]
        public async Task<IActionResult> DeactivateTimer(CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();

                await _clusterClient.GetGrain<ITimer>(1)
                    .DeactivateTimer(grainCancellationTokenSource.Token);

                result = Ok("Timer deactivated");
            }

            return result;
        }

        [HttpGet("deactivate-grain")]
        public async Task<IActionResult> DeactivateGrain(CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();

                await _clusterClient.GetGrain<ITimer>(1)
                    .DeactivateGrain(grainCancellationTokenSource.Token);

                result = Ok("Grain activation deactivated");
            }

            return result;
        }
    }
}