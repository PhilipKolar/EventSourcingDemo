using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Data.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public int ProductId { get; set; }
        public int? AddonId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
    }
}
