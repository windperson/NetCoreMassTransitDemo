using HostedServiceDemo.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace HostedServiceDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logConfig = new LoggerConfiguration()
                //.MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Trace()
                .WriteTo.Debug();

            Log.Logger = logConfig
                .Enrich.FromLogContext().CreateLogger();

            var builder = new HostBuilder().ConfigureServices((hostContext, services) => {
                services.AddHostedService<MessageQueueService>();
            });

            await builder.RunConsoleAsync();
        }
    }
}
