using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace EventSourcingDemo.Models.Response
{
    public class CreateOrderResponse
    {
        public int OrderId { get; set; }
    }
}
