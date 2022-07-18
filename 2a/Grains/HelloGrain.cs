using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;
using Configuration;
using Interfaces;
using Orleans;
using Orleans.Providers;
using Serilog;
using Serilog.Core;

namespace Grains
{
    public class HelloGrain : Grain, IHello
    {
        public HelloGrain()
        {
            Log.Logger = TelemetryManager.SerilogConfiguration();
        }

        public Task<string> PrintHello(string greeting)
        {
            Log.Information("GRAIN: {Class}.SayHello\n", GetType().Name);
            return Task.FromResult($"Function says: {greeting}");
        }

        public async Task<string> TraceHello(int milliSeconds)
        {
            using var activity = TelemetryManager.ActivitySource?.StartActivity($"Grains.HelloGrain.WaitHello.Span");
            activity?.SetTag("foo", "bar");
            activity?.AddEvent(new ActivityEvent("Function starting ..."));
                
            Log.Information("GRAIN: {Class}.WaitHello\n", GetType().Name);
            await Task.Delay(milliSeconds);
            activity?.AddEvent(new ActivityEvent("Function complete"));
            return $"Function waited: {milliSeconds} milliseconds";
        }

        public async Task MetricHello(int iterations)
        {
            var counter = TelemetryManager.Meter?.CreateCounter<int>("Grains.HelloGrain.MetricHello.Counter");
            var histogram = TelemetryManager.Meter?.CreateHistogram<double>("Grains.HelloGrain.MetricHello.Histogram", unit: "ms");
            TelemetryManager.Meter?.CreateObservableGauge("Grains.HelloGrain.MetricHello.Gauge", () => new[] { new Measurement<int>(ThreadPool.ThreadCount) });
            var index = 0;
            Log.Information("GRAIN: {Class}.MetricHello\n", GetType().Name);

            for (var i = 0; i < iterations; i++)
            {
                await Task.Delay(200);
                index++;
                counter?.Add(i, KeyValuePair.Create<string, object?>("title", i.ToString()));
            }
            
            histogram?.Record(index, tag: KeyValuePair.Create<string, object?>("Grains.HelloGrain.MetricHello.Counter.Count", DateTime.UtcNow));
        }
    }
}