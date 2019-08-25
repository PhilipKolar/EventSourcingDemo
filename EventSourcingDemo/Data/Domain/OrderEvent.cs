using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Data.Domain
{
    public class OrderEvent
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTimeOffset DateOccurred { get; set; }
        public int EventId { get; set; }
        public string Payload { get; set; }
    }
}
