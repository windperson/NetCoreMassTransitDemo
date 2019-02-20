using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Hosting;
using SharedContract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HostedServiceDemo.Consumer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HostedServiceDemo.HostedServices
{
    class MessageQueueService : BackgroundService
    {
        private readonly ILogger<MessageQueueService> _logger;

        private readonly IBusControl _bus;

        public MessageQueueService(IBusControl busControl, ILogger<MessageQueueService> logger)
        {
            _logger = logger;

            _bus = busControl;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start service bus...");
            return _bus.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync(cancellationToken));
        }
    }
}
