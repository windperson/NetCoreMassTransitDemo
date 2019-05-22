using CourierPatternDemo.MsgContracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierPatternDemo.MsgConsumers
{
    public class OrderProcessing : IConsumer<IOrderRequest>
    {
        public Task Consume(ConsumeContext<IOrderRequest> context)
        {
            throw new NotImplementedException();
        }
    }
}
