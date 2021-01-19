using System.Threading.Tasks;
using Microsoft.FeatureManagement;
using Moq;
using Orleans;
using Orleans.TestKit;
using Xunit;

namespace Grains.UnitTests
{
    //Unlike the standard orleans testing libraries, this allows you to run a simple unit test with dependencies.
    // This means full silos don't have to be created which results in faster and easier to implement tests
    public class SampleDependencyUnitTest:TestKitBase
    {
        [Fact]
        public async Task InjectedDependencyCanBeVerified()
        {
            //This creates a mock and injects it into the tested grain.
            var featureManagementService = Silo.AddServiceProbe<IFeatureManagerSnapshot>();
            featureManagementService.Setup(x => x.IsEnabledAsync("FeatureA"))
                .ReturnsAsync(true);
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<HelloWorld>(1);
            
            await sut.SayHello("Mike", token);
            
            featureManagementService.Verify(x => x.IsEnabledAsync("FeatureA"), Times.Once);
        }
    }
}