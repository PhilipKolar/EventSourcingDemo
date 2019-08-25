using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Models.Response;
using MediatR;

namespace EventSourcingDemo.Models.Request
{
    public class CreateOrderRequest : IRequest<CreateOrderResponse>
    {
        public int ProductId { get; set; }
        public int? AddonId { get; set; }
    }
}
