using System;
using System.Collections.Generic;
using System.Linq;
using Orleans.Runtime;
using StatsdClient;

namespace SiloHost
{
public class DatadogTelemetryConsumer : IMetricTelemetryConsumer, IDisposable
{
    private readonly DogStatsdService _service;

    public DatadogTelemetryConsumer()
    {
        var statsdConfig = new StatsdConfig();

        _service = new DogStatsdService();
        _service.Configure(statsdConfig);
    }

    public void DecrementMetric(string name) => _service.Decrement(name);

    public void DecrementMetric(string name, double value) => _service.Decrement(name, Clamp(value));

    public void IncrementMetric(string name) => _service.Increment(name);

    public void IncrementMetric(string name, double value) => _service.Increment(name, Clamp(value));

    public void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null) =>
        _service.Distribution(name, value.TotalMilliseconds, tags: ToTags(properties));

    public void TrackMetric(string name, double value, IDictionary<string, string> properties = null) =>
        _service.Distribution(name, value, tags: ToTags(properties));

    public void Flush() => _service.Flush();

    public void Close() => _service.Flush();

    public void Dispose() => _service.Dispose();

    private int Clamp(double value) => (int) Math.Clamp(value, int.MinValue, int.MaxValue);

    private string[] ToTags(IDictionary<string, string> properties) => properties?.Select(ToTag).ToArray();

    private static string ToTag(KeyValuePair<string, string> property) => $"{property.Key.Replace(':', '_')}:{property.Value.Replace(':', '_')}";
}
}