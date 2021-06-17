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

        private readonly string[] _metricPrefixes = {"App.Requests.", "Grain."};

        public DatadogTelemetryConsumer()
        {
            var config = new StatsdConfig();
            _service = new DogStatsdService();
            _service.Configure(config);
        }

        public void DecrementMetric(string name)
        {
            if (ShouldRecord(name))
            {
                _service.Decrement(FormatMetricName(name));
            }
        }

        public void DecrementMetric(
            string name,
            double value)
        {
            if (ShouldRecord(name))
            {
                _service.Decrement(
                    FormatMetricName(name),
                    (int) Math.Clamp(
                        value,
                        int.MinValue,
                        int.MaxValue));
            }
        }

        public void IncrementMetric(string name)
        {
            if (ShouldRecord(name))
            {
                _service.Increment(FormatMetricName(name));
            }
        }

        public void IncrementMetric(
            string name,
            double value)
        {
            if (ShouldRecord(name))
            {
                _service.Increment(
                    FormatMetricName(name),
                    (int) Math.Clamp(
                        value,
                        int.MinValue,
                        int.MaxValue));
            }
        }

        public void TrackMetric(
            string name,
            TimeSpan value,
            IDictionary<string, string> properties = null)
        {
            if (ShouldRecord(name))
            {
                _service.Distribution(
                    FormatMetricName(name),
                    value.TotalMilliseconds,
                    tags: GetTags(properties)?.ToArray());
            }
        }

        public void TrackMetric(
            string name,
            double value,
            IDictionary<string, string> properties = null)
        {
            if (ShouldRecord(name))
            {
                _service.Distribution(
                    FormatMetricName(name),
                    value,
                    tags: GetTags(properties)?.ToArray());
            }
        }

        public void Flush() => _service.Flush();

        public void Close() => _service.Flush();

        public void Dispose() => _service.Dispose();

        // private bool ShouldRecord(string name) => _metricPrefixes.Any(name.StartsWith);

        private bool ShouldRecord(string name) => true;

        private string FormatMetricName(string name) => $"orleans.{name}";

        private IEnumerable<string> GetTags(IDictionary<string, string> properties)
        {
            return properties?
                .Select(kvp =>
                    $"{kvp.Key.Replace(':', '_')}:{kvp.Value.Replace(':', '_')}");
        }
    }
}