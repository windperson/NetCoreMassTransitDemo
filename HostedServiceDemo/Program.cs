using HostedServiceDemo.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;
using HostedServiceDemo.Consumer;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using MassTransit.SerilogIntegration;
using SharedContract;

namespace HostedServiceDemo
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.Trace()
                .WriteTo.Debug()
                .Enrich.FromLogContext().CreateLogger();

            var builder = CreateHostBuilder(args);

            await builder.RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(cfg =>
                    {
                        cfg.AddConsumer<OrderConsumer>();
                        cfg.AddBus(AzureServiceBusFactory);
                    });

                    services.AddHostedService<MessageQueueService>();
                })
                .UseSerilog();


        private static IBusControl AzureServiceBusFactory(IServiceProvider provider) =>
            Bus.Factory.CreateUsingAzureServiceBus(config =>
            {
                config.UseSerilog();
                var host = config.Host(new Uri(ConstantForAzureServiceBus.ServiceBusUrl), hostCfg =>
                {
                    hostCfg.TokenProvider =
                        TokenProvider.CreateSharedAccessSignatureTokenProvider(
                            ConstantForAzureServiceBus.KeyName,
                            @"Nk8YFq5WKvVFM0sp+DAh9spopa/gUxBYQps0VF4rJi4=");

                    //hostCfg.OperationTimeout = TimeSpan.FromSeconds(10);
                });

                config.ReceiveEndpoint(host, Constant.DemoQueueName, configurator =>
                {
                    configurator.ConfigureConsumer<OrderConsumer>(provider);
                });
            });
    }
}
