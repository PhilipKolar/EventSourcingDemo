﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Data.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
