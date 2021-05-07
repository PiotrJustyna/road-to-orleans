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
    public class TimerWorldController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;
        private readonly Random _generator;

        public TimerWorldController(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
            _generator = new Random();
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateTimer(CancellationToken cancellationToken)
        {
            IActionResult result = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                var grainCancellationTokenSource = new GrainCancellationTokenSource();

                //A random integer (1 - 2) is generated to allow for a new timer world grain to be created or reused per client.
                var timerActivationResult = await _clusterClient.GetGrain<ITimerWorld>(_generator.Next(1, 3))
                    .ActivateTimer(grainCancellationTokenSource.Token);

                result = Ok(timerActivationResult);
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
                
                //A random integer (1 - 2) is generated to allow for a new timer world grain to be created or reused per client.
                var timerDeactivationResult = await _clusterClient.GetGrain<ITimerWorld>(_generator.Next(1, 3))
                    .DeactivateTimer(grainCancellationTokenSource.Token);

                result = Ok(timerDeactivationResult);
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

                //A random integer (1 - 2) is generated to allow for a new timer world grain to be created or reused per client.
                var grainDeactivationResult = await _clusterClient.GetGrain<ITimerWorld>(_generator.Next(1, 3))
                    .DeactivateGrain(grainCancellationTokenSource.Token);

                result = Ok(grainDeactivationResult);
            }

            return result;
        }
    }
}