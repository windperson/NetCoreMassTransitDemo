using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourierPatternDemo.MsgContracts;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;

namespace CourierPatternDemo.CourierActivities
{
    public class DeliverProductActivity : Activity<IDeliverProductArgument, IDeliverProductLog>
    {
        private readonly ILogger<DeliverProductActivity> _logger;

        public DeliverProductActivity(ILogger<DeliverProductActivity> logger)
        {
            _logger = logger;
        }


        public Task<ExecutionResult> Execute(ExecuteContext<IDeliverProductArgument> context)
        {
            //make product delivery failed at 1/3 chance.
            var random = new Random(unchecked((int)DateTime.Now.Ticks));
            if (random.Next() % 3 == 0)
            {
                _logger.LogWarning("Random Failed Happened!!!");
                return Task.FromResult(context.Faulted(new Exception("Random chaos monkey coming!!!")));
            }

            _logger.LogInformation("Ordcer Id: {@1} deiliver success.", context.Arguments.PurchaseTransactionId);
            return Task.FromResult(context.Completed());
        }

        public Task<CompensationResult> Compensate(CompensateContext<IDeliverProductLog> context)
        {
            _logger.LogWarning("{@1}: product delivering error, log={@2}", context.ExecutionId, context.Log);
            return Task.FromResult(context.Compensated());
        }
    }
}
