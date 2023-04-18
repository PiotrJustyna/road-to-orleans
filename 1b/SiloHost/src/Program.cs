using System;
using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;

[assembly: GenerateCodeForDeclaringAssembly(typeof(Grains.YourReminderGrain))]

using var host = new HostBuilder()
    .UseOrleans(builder =>
    {
        builder.UseLocalhostClustering();
        builder.UseInMemoryReminderService();
        builder.UseDashboard();
        builder.ConfigureServices(_ => _.AddHostedService<GrainActivatorHostedService.GrainActivatorHostedService>());
    })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

Console.ReadLine();