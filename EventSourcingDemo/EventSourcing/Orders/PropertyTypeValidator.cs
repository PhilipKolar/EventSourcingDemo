using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using EventSourcingDemo.EventSourcing.Orders.Properties;

namespace EventSourcingDemo.EventSourcing.Orders
{
    public class PropertyTypeValidator
    {
        public Dictionary<OrderProperty, List<Type>> ValidTypes { get; private set; }

        public PropertyTypeValidator()
        {
            ValidTypes = new Dictionary<OrderProperty, List<Type>>
            {
                { OrderProperty.Product, new List<Type> { typeof(Product) } },
                { OrderProperty.Addon, new List<Type> { typeof(Addon) } },
                { OrderProperty.TotalPrice, new List<Type> { typeof(decimal) } }
            };
        }


        public bool IsValid(OrderProperty orderProperty, Type valueType)
        {
            if (!ValidTypes.ContainsKey(orderProperty))
            {
                throw new NotImplementedException("Unknown Order Property");
            }

            return ValidTypes[orderProperty].Contains(valueType);
        }
    }
}
