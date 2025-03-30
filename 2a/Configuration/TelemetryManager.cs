using System.Diagnostics;
using System.Diagnostics.Metrics;
using Serilog;
using Serilog.Core;

namespace Configuration;

public static class TelemetryManager
{
    public static ActivitySource? ActivitySource;
    public static Meter? Meter;

    public static void InitializeTraceAndMeter(string traceName, string meterName)
    {
        InitializeTrace(traceName);
        InitializeMeter(meterName);
    }
    
    public static void InitializeTrace(string name)
    {
        //Setting DefaultIdFormat and ForceDefaultIdFormat is optional
        //but helps ensure the sample produces similar output on different .NET runtime versions
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;
        Activity.ForceDefaultIdFormat = true;
        //OpenTelemetry 'Tracer' = .NET ActivitySource.
        //OpenTelemetry 'Span = .NET Activity.
        ActivitySource = new ActivitySource(name);

        ActivitySource.AddActivityListener(new ActivityListener()
        {
            ShouldListenTo = _ => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity => Console.WriteLine("Started: {0} {1} {2} {3} {4}", activity.Source.Name, activity.OperationName, activity.Id, activity.Duration, activity.TagObjects.Count()),
            ActivityStopped = activity => Console.WriteLine("Stopped: {0} {1} {2} {3} {4}", activity.Source.Name, activity.OperationName, activity.Id, activity.Duration, activity.TagObjects.Count())
        });
    }

    public static void InitializeMeter(string name)
    {
        Meter = new Meter(name);
    }

    public static Logger SerilogConfiguration()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    } 
}