using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.EventSourcing.Orders
{
    public enum OrderEventType
    {
        Created = 1,
        PropertyUpdated,
        Completed,
        Cancelled
    }
}
