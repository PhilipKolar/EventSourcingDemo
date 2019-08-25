using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Enums
{
    public enum OrderStatus
    {
        Requested = 1,
        Completed,
        Cancelled
    }
}
