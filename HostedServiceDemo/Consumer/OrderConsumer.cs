using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using Microsoft.Extensions.Logging;
using SharedContract;

namespace HostedServiceDemo.Consumer
{
    public class OrderConsumer : IConsumer<ISubmitOrder>
    {
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }


        public Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            var receiveData = context.Message;
            _logger.LogInformation("receive data: {@1}", receiveData);
            return context.RespondAsync<IOrderAccepted>(new { receiveData.OrderId });
        }
    }
}