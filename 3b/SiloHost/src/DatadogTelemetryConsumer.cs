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

        private readonly string[] _namesOfRelevantMetrics;

        /// <summary>
        /// Default constructor.
        /// To be used when no configuration is required.
        /// In those cases, explicit instantiation is not required,
        /// Orleans is able to instantiate the class on its own.
        /// </summary>
        public DatadogTelemetryConsumer()
        {
            var statsdConfig = new StatsdConfig();

            _service = new DogStatsdService();
            _service.Configure(statsdConfig);
        }

        /// <summary>
        /// Preferred constructor allowing for a degree of configuration.
        /// To be used explicitly while configuring the IoC services.
        /// </summary>
        /// <param name="namesOfRelevantMetrics">
        /// Names of metrics to track.
        /// If null is provided, every metric name is tracked.
        /// Careful, there could be tens of metrics tracked!
        /// Metric names are case sensitive.
        /// </param>
        public DatadogTelemetryConsumer(string[] namesOfRelevantMetrics) : this()
        {
            _namesOfRelevantMetrics = namesOfRelevantMetrics;
        }

        public void DecrementMetric(string name)
        {
            if (IsRelevant(name)) _service.Decrement(name);
        }

        public void DecrementMetric(string name, double value)
        {
            if (IsRelevant(name)) _service.Decrement(name, Clamp(value));
        }

        public void IncrementMetric(string name)
        {
            if (IsRelevant(name)) _service.Increment(name);
        }

        public void IncrementMetric(string name, double value)
        {
            if (IsRelevant(name)) _service.Increment(name, Clamp(value));
        }

        public void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null)
        {
            if (IsRelevant(name)) _service.Distribution(name, value.TotalMilliseconds, tags: ToTags(properties));
        }

        public void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            if (IsRelevant(name)) _service.Distribution(name, value, tags: ToTags(properties));
        }

        public void Flush() => _service.Flush();

        public void Close() => _service.Flush();

        public void Dispose() => _service.Dispose();

        private int Clamp(double value) => (int) Math.Clamp(value, int.MinValue, int.MaxValue);

        private string[] ToTags(IDictionary<string, string> properties) => properties?.Select(ToTag).ToArray();

        private static string ToTag(KeyValuePair<string, string> property) =>
            $"{property.Key.Replace(':', '_')}:{property.Value.Replace(':', '_')}";

        /// <summary>
        /// Metric name is considered relevant if it exists in the collection of configured names.
        /// If the collection is not configured, all metrics are tracked.
        /// </summary>
        /// <param name="metricName">Name of a metric to verify. E.g. App.Requests.Total.Requests.Current.</param>
        /// <returns>
        /// True - metric is relevant and will be tracked.
        /// False - metric is not relevant and will not be tracked.
        /// </returns>
        private bool IsRelevant(string metricName) => _namesOfRelevantMetrics?.Contains(metricName) ?? true;
    }
}