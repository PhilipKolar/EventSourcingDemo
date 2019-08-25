using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Data;
using EventSourcingDemo.Data.Domain;
using EventSourcingDemo.Util;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Product = EventSourcingDemo.EventSourcing.Orders.Properties.Product;

namespace EventSourcingDemo.EventSourcing.Orders
{
    public class OrderStateRebuilder
    {
        private readonly PropertyTypeValidator _propertyValidator;
        private readonly IClock _clock;
        private readonly PkStoreContext _context;

        public OrderStateRebuilder(PropertyTypeValidator propertyValidator, IClock clock, PkStoreContext context)
        {
            _propertyValidator = propertyValidator;
            _clock = clock;
            _context = context;
        }

        public async Task<Order> Rebuild(int orderId)
        {
            return await Rebuild(orderId, _clock.UtcNow);
        }

        public async Task<Order> Rebuild(int orderId, DateTimeOffset asOf)
        {
            var allEvents = await _context.OrderEvents
                .Where(x => x.OrderId == orderId && x.DateOccurred <= asOf)
                .OrderBy(x => x.DateOccurred)
                .ToListAsync();

            var newOrder = new Order();
            newOrder.Id = orderId;
            foreach (var orderEvent in allEvents)
            {
                switch (orderEvent.EventId)
                {
                    case (int)OrderEventType.Created:
                        HandleCreated(newOrder, orderEvent);
                        break;
                    case (int)OrderEventType.PropertyUpdated:
                        HandlePropertyUpdated(newOrder, orderEvent);
                        break;
                    default:
                        throw new NotImplementedException("Unknown order event");
                }
            }

            return newOrder;
        }

        private void HandleCreated(Order order, OrderEvent orderEvent)
        {
            order.Status = (int)Enums.OrderStatus.Requested;
            order.DateCreated = orderEvent.DateOccurred;
        }

        private void HandlePropertyUpdated(Order newOrder, OrderEvent orderEvent)
        {
            var payload = JsonConvert.DeserializeObject<PropertyUpdatedPayload>(orderEvent.Payload);

            //TODO: Refactor this delicious spaghetti
            if (payload.Name == Enum.GetName(typeof(OrderProperty), OrderProperty.Product))
            {
                if (payload.Type == typeof(Product).FullName)
                {
                    var castValue = ((JObject)payload.Value).ToObject<Product>();
                    newOrder.ProductId = castValue.Id;
                }
                else
                {
                    throw new NotSupportedException($"Unknown payload type ${payload.Type}");
                }
            } else if (payload.Name == Enum.GetName(typeof(OrderProperty), OrderProperty.Addon))
            {
                if (payload.Type == typeof(Properties.Addon).FullName)
                {
                    var castValue = ((JObject)payload.Value).ToObject<Properties.Addon>();
                    newOrder.AddonId = castValue.Id;
                }
                else
                {
                    throw new NotSupportedException($"Unknown payload type ${payload.Type}");
                }
            } else if (payload.Name == Enum.GetName(typeof(OrderProperty), OrderProperty.TotalPrice))
            {
                if (payload.Type == typeof(decimal).FullName)
                {
                    var castValue = (decimal)(double)payload.Value;
                    newOrder.TotalPrice = castValue;
                }
                else
                {
                    throw new NotSupportedException($"Unknown payload type ${payload.Type}");
                }
            }
        }
    }
}
