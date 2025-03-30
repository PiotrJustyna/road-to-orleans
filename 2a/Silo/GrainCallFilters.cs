using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using Orleans;
using Serilog;
using Configuration;

namespace Silo
{
    internal class IncomingGrainCallFilter : IIncomingGrainCallFilter
    {
        private static readonly HashSet<string> AcceptedAssemblyNames = new()
        {
            "Grains"
        };

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                if (AcceptedAssemblyNames.Contains(context.Grain.GetType().Namespace))
                {
                    var grain = context.Grain.GetType();
                    
                    //OpenTelemetry uses alternate terms 'Tracer' and 'Span'.
                    //In .NET 'ActivitySource' is the implementation of Tracer and Activity is the implementation of 'Span'.
                    using (TelemetryManager.ActivitySource?.StartActivity($"{grain.FullName}.IncomingGrainCallFilter.Span"))
                    {
                        await context.Invoke();
                        Log.Information(
                            "FILTER: \nAssemblyNamespace: {AssemblyNamespace}\nFullName: {FullName}\nModuleName: {ModuleName}\nInterfaceName: {InterfaceName}\nResult: {Result}\n",
                            grain.Namespace,
                            grain.FullName,
                            grain.Module.Name,
                            context.InterfaceMethod.Name,
                            context.Result);
                    }
                }
                else
                {
                    await context.Invoke();
                }
            }
            catch (Exception exception)
            {
                Log.Error("An error was thrown: {Message}", exception.Message);

                // If this exception is not re-thrown, it is considered to be
                // handled by this filter.
                throw;
            }
        }
    }
}