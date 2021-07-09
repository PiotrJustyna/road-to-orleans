# readme

This example builds on top of Solution 3 by introducing datadog as an APM tool. The goal is to demonstrate how to:

* send orleans metrics to datadog
* implement `IMetricTelemetryConsumer` with datadog

## how it works

### datadog

In addition to docker images defined in solution 3, we now have one more, which is hosting the datadog agent. The way datadog works is that it needs:

* tracer - this runs alongside the executable code it's supposed to monitor (in the same container)
* agent - this runs as a separate container alongside the container hosting your monitored application

We now have a [docker-compose.yml](./ops/SiloHost/docker-compose.yml) file which starts the datadog agent & silo host  duo.

### consumer

The consumer has two constructors:

* `DatadogTelemetryConsumer(string[] namesOfRelevantMetrics, StatsdConfig statsdConfig)` - accepts a collection of metric names to be tracked and datadog statsd configuration

  ```c#
    siloBuilder.ConfigureServices(services =>
    {
      services.AddSingleton(serviceProvider =>
          new DatadogTelemetryConsumer(
              new[] {"App.Requests.Total.Requests.Current"},
              new StatsdConfig
              {
                  StatsdServerName = "127.0.0.1",
                  StatsdPort = 8125
              }));
      services.Configure<TelemetryOptions>(
          telemetryOptions => telemetryOptions.AddConsumer<DatadogTelemetryConsumer>());
    });
  ```

  Here, Orleans will detect that `DatadogTelemetryConsumer` is already registered in the collection of silo's services and will not instantiate it again.

  More on statsd configuration here: https://docs.datadoghq.com/developers/service_checks/dogstatsd_service_checks_submission/

* `DatadogTelemetryConsumer()` - Orleans uses this one by default if `DatadogTelemetryConsumer` instance cannot be found in the collection of silo's services.

  ```c#
  siloBuilder.ConfigureServices(services =>
  {
      services.Configure<TelemetryOptions>(
          telemetryOptions => telemetryOptions.AddConsumer<DatadogTelemetryConsumer>());
  });
  ```
  
  Here, Orleans will instantiate `DatadogTelemetryConsumer` using the default constructor.

## running the code

The easiest way to run the code is to execute:

* run-silo-docker.sh - this executes the [docker-compose.yml](./ops/SiloHost/docker-compose.yml) file and builds not only the silo host, but agent container propagating the data to datadog
  ![](./img/docker-compose-output.png)
* run-client-docker.sh - this starts the orleans client, no changes there

The code can also be ran locally or in remote containers provided that the `SiloHost` can communicate with datadog agent.

## metrics

There are 167 metrics the telemetry consumer exposes:

![all metrics](./img/metrics-summary.png)

They are all correctly tagged:

![metric tags](./img/tagged-metric.png)

Captured metrics help us formulate good understanding of what our orleans application is doing and what the resource consumption is. To list some of the most useful metrics:

|||
| --- | --- |
| `Grain.Grains.HelloWorld.Current` - number of `HelloWorld` grain. In our case it's an uneventful one grain. ![](./img/Grain.Grains.HelloWorld.Current.png) | `App.Requests.Latency.Average.Millis.Current` - average application latency. ![](./img/App.Requests.Latency.Average.Millis.Current.png) |
| `App.Requests.Total.Requests.Current` - total number of requests an application receives. In this example the application was restarted which reset the counter to 0. ![](./img/App.Requests.Total.Requests.Current.png) | `Runtime.Memory.AvailableMemoryMb.Current` - amount of available memory in Mb. ![](./img/Runtime.Memory.AvailableMemoryMb.Current.png) |
| `Runtime.CpuUsage.Current` - amount of CPU the runtime consumes. ![](./img/Runtime.CpuUsage.Current.png) | `App.Requests.TimedOut.Current` - number of timed out requests. Uneventful zero. ![](./img/App.Requests.TimedOut.Current.png) |

If `DatadogTelemetryConsumer` is configured not to track all metrics (the screenshot below shows only `App.Requests.Total.Requests.Current` being tracked), the list looks like this:

![filtered-metrics](./img/filtered-metrics.png)