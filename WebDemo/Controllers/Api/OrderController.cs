using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedContract;

namespace WebDemo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<ISubmitOrder, IOrderAccepted> _requestClient;

        private ILogger<OrderController> _logger { get; }

        public OrderController(IRequestClient<ISubmitOrder, IOrderAccepted> requestClient, ILogger<OrderController> logger)
        {
            _requestClient = requestClient;
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Submit(string id)
        {
            try
            {
                var result = await _requestClient.Request(new { OrderId = id });
                return Accepted(result.OrderId);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.LogError(ex, "Azure Service Bus Request timeout");
                throw;
            }
        }
    }
}