using Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grains
{
    public class Timer : Grain, ITimer
    {
        private readonly ILogger<Timer> _logger;
        private IDisposable _timer;
        private int _executionCount;

        public Timer(ILogger<Timer> logger)
        {
            _logger = logger;
        }

        public Task ActivateTimer(GrainCancellationToken grainCancellationToken)
        {
            if (!grainCancellationToken.CancellationToken.IsCancellationRequested && _timer == null)
            {
                _timer = RegisterTimer(TimerCallback, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1));
            }

            return Task.CompletedTask;
        }

        public Task DeactivateTimer(GrainCancellationToken grainCancellationToken)
        {
            if (!grainCancellationToken.CancellationToken.IsCancellationRequested && _timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            return Task.CompletedTask;
        }

        public Task DeactivateGrain(GrainCancellationToken grainCancellationToken)
        {
            if (!grainCancellationToken.CancellationToken.IsCancellationRequested)
            {
                DeactivateOnIdle();
            }

            return Task.CompletedTask;
        }

        private async Task TimerCallback(object state)
        {
            _executionCount++;

            var nowInSeconds = DateTime.UtcNow.Second;

            _logger.LogInformation($"{nowInSeconds}: This timer has executed {_executionCount} times - {IdentityString}");

            // this is just to demonstrate that the timer waits for `period`
            // after the execution finishes before starting the next one
            // meaning that the log above will happen every 3 seconds
            await Task.Delay(2000);
        }
    }
}