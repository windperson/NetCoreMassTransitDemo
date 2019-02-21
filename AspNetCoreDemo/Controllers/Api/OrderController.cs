using System;
using System.Threading.Tasks;
using AspNetCoreDemo.Constant;
using AspNetCoreDemo.MsgContracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreDemo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IBus _bus;

        public OrderController(IBus bus, ILogger<OrderController> logger)
        {
            _bus = bus;
            
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Submit(string id)
        {
            try
            {
                var sendOrder = new SendOrder {OrderId = id, OrderDate = DateTime.Now, OrderAmount = 1};
                _logger.LogInformation("sending data: {@1}", sendOrder);

                await _bus.Send(sendOrder);
                
                return Accepted(id);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Bus Request timeout");
                throw;
            }
        }
    }
}