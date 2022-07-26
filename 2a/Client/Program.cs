using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Interfaces;
using Serilog;

namespace Client
{
    public static class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();
                
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .Build();

            await client.Connect();
            Log.Information("Client successfully connected to silo host");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var hello = client.GetGrain<IHello>(0);
            var response1 = await hello.PrintHello("Good morning, HelloGrain!");
            Log.Information("{Response}", response1);

            var response2 = await hello.TraceHello(2000);
            Log.Information("{Response}", response2);

            await hello.MetricHello(3);
        }
    }
}