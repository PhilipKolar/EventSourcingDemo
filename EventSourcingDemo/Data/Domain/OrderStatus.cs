﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingDemo.Data.Domain
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
