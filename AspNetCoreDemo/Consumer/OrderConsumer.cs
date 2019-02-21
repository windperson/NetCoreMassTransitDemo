using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreDemo.MsgContracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemo.Consumer
{
    public class OrderConsumer : IConsumer<ISendOrder>
    {
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISendOrder> context)
        {
            var receiveData = context.Message;
            _logger.LogInformation("receive data: {@1}", receiveData);

        }
    }
}
