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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Trace()
                .WriteTo.Debug()
                .Enrich.FromLogContext().CreateLogger();

            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<MessageQueueService>();
            }).UseSerilog();

            await builder.RunConsoleAsync();
        }
    }
}
