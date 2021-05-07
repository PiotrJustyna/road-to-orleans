using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class TimerWorld : Grain, ITimerWorld
    {
        private readonly ILogger<TimerWorld> _logger;
        private IDisposable _timer;
        private int _executionCount;

        public TimerWorld(ILogger<TimerWorld> logger)
        {
            _logger = logger;
        }

        public Task<string> ActivateTimer(GrainCancellationToken grainCancellationToken)
        {
            if (grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                return Task.FromResult("Cancellation requested");
            }

            string result;

            if (_timer == null)
            {
                _timer = RegisterTimer(TimerCallback, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1));
                result = $"Timer registered - {IdentityString}";
            }
            else
            {
                result = $"Timer already registered - {IdentityString}";
            }

            return Task.FromResult(result);
        }

        public Task<string> DeactivateTimer(GrainCancellationToken grainCancellationToken)
        {
            if (grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                return Task.FromResult("Cancellation requested");
            }

            string result;

            if (_timer == null)
            {
                result = $"Timer is not registered - {IdentityString}";
            }
            else
            {
                _timer.Dispose();
                _timer = null;
                result = $"Timer unregistered - {IdentityString}";
            }

            return Task.FromResult(result);
        }

        public Task<string> DeactivateGrain(GrainCancellationToken grainCancellationToken)
        {
            if (grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                return Task.FromResult("Cancellation requested");
            }

            string result;

            if (_timer == null)
            {
                result = $"Timer is not registered - {IdentityString}";
            }
            else
            {
                DeactivateOnIdle();
                result = $"Grain deactivation requested - {IdentityString}";
            }

            return Task.FromResult(result);
        }

        private async Task TimerCallback(object state)
        {
            _executionCount++;

            var nowInSeconds = DateTime.UtcNow.Second;

            _logger.LogInformation($"{nowInSeconds}: This timer has executed {_executionCount} times - {IdentityString}");

            // this is just to demonstrate that the timer waits for `period`
            // after the execution finishes before starting the next one
            // meaning that the log above will happen every 5 seconds
            await Task.Delay(4000);
        }
    }
}