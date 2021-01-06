using System.Threading.Tasks;
using Grains;
using Microsoft.FeatureManagement;
using Moq;
using Orleans;
using Orleans.TestKit;
using Xunit;

namespace GrainUnitTests
{
    //Unlike the standard orleans testing libraries, this allows you to run a simple unit test with dependencies.
    // This means full silos don't have to be created which results in faster and easier to implement tests
    public class SampleDependencyUnitTest:TestKitBase
    {
        [Fact]
        public async Task InjectedDependencyReturnsExpectedResponse()
        {
            var expectedResponse = "Hello, \"Mike\"! Your name is 4 characters long. Grain reference - 1";
            var featureManagementService = Silo.AddServiceProbe<IFeatureManagerSnapshot>();
            featureManagementService.Setup(x => x.IsEnabledAsync("DummyFeatureA"))
                .ReturnsAsync(true);
            var token = new GrainCancellationTokenSource().Token;
            
            var sut = await Silo.CreateGrainAsync<HelloWorld>(1);
            var result = await sut.SayHello("Mike", token);
            
            Assert.Equal(expectedResponse, result);
        }
        
        [Fact]
        public async Task InjectedDependencyCanBeVerified()
        {
            var featureManagementService = Silo.AddServiceProbe<IFeatureManagerSnapshot>();
            featureManagementService.Setup(x => x.IsEnabledAsync("DummyFeatureA"))
                .ReturnsAsync(true);
            var token = new GrainCancellationTokenSource().Token;
            var sut = await Silo.CreateGrainAsync<HelloWorld>(1);
            
            await sut.SayHello("Mike", token);
            
            featureManagementService.Verify(x => x.IsEnabledAsync("DummyFeatureA"), Times.Once);
        }
    }
}