﻿using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedContract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceDemo.HostedServices
{
    class MessageQueueService : BackgroundService
    {
        private readonly ILogger<MessageQueueService> _logger;

        private readonly IBusControl _bus;

        public MessageQueueService(ILogger<MessageQueueService> logger)
        {
            _logger = logger;

            _bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(new Uri(ConstantForAzureServiceBus.ServiceBusUrl), hostCfg =>
                {
                    hostCfg.OperationTimeout = TimeSpan.FromSeconds(10);
                    hostCfg.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConstantForAzureServiceBus.KeyName, @"Nk8YFq5WKvVFM0sp+DAh9spopa/gUxBYQps0VF4rJi4=");
                });

                cfg.ReceiveEndpoint(host, Constant.DemoQueueName, efg =>
                {
                    efg.Handler<SubmitOrder>(context => {
                        _logger.LogInformation($"recevie {nameof(SubmitOrder)} data");
                        return context.RespondAsync<OrderAccepted>(new { context.Message.OrderId });
                    });
                });
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _bus.StartAsync();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync());
        }
    }
}
