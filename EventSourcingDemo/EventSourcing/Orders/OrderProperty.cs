using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.EventSourcing.Orders
{
    public enum OrderProperty
    {
        Product,
        Addon,
        TotalPrice
    }
}
