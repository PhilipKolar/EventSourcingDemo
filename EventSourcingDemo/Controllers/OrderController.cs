using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Models.Request;
using EventSourcingDemo.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CreateOrderResponse> Create([FromBody] CreateOrderRequest request)
        {
            var response = await _mediator.Send(request);
            return response;
        }
    }
}
