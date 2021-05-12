using Microsoft.Extensions.Logging;
using Moq;
using Orleans;
using Orleans.TestKit;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Grains.UnitTests
{
    public class TimerUnitTest : TestKitBase
    {
        [Fact]
        public async Task TimerCanBeRegistered()
        {
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<Timer>(1);

            await sut.ActivateTimer(token);

            Silo.TimerRegistry.Mock.Verify(x => x.RegisterTimer(It.IsAny<Timer>(),
                It.IsAny<Func<object, Task>>(), null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1)), Times.Once);
        }

        [Fact]
        public async Task TimerCanBeFiredAfterRegistration()
        {
            var logger = Silo.AddServiceProbe<ILogger<Timer>>();
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<Timer>(1);

            await sut.ActivateTimer(token);
            await Silo.FireAllTimersAsync();

            Assert.Equal(1, logger.Invocations.Count);
        }

        [Fact]
        public async Task TimerCanBeUnregistered()
        {
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<Timer>(1);

            await sut.ActivateTimer(token);
            await sut.DeactivateTimer(token);

            var field = sut.GetType().GetField("_timer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(sut);
            Assert.Null(field);
        }

        [Fact]
        public async Task TimerCanBeUnregisteredByGrainDeactivation()
        {
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<Timer>(1);

            await sut.ActivateTimer(token);
            await sut.DeactivateGrain(token);

            Silo.VerifyRuntime(x => x.DeactivateOnIdle(sut), Times.Once);
        }
    }
}