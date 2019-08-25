using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingDemo.Data;
using EventSourcingDemo.Data.Domain;
using EventSourcingDemo.EventSourcing.Orders;
using EventSourcingDemo.Models.Request;
using EventSourcingDemo.Models.Response;
using EventSourcingDemo.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EventSourcingDemo.RequestHandlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly IClock _clock;
        private readonly PkStoreContext _context;
        private readonly OrderStateRebuilder _orderStateRebuilder;

        public CreateOrderHandler(IClock clock, PkStoreContext context, OrderStateRebuilder orderStateRebuilder)
        {
            _clock = clock;
            _context = context;
            _orderStateRebuilder = orderStateRebuilder;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var eventTime = _clock.UtcNow;
            var orderId = -1;
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    orderId = await CreateCreatedEvent(eventTime, cancellationToken);
                    var totalPrice = await GetTotalPriceAsync(request, cancellationToken);
                    await CreatePropertyUpdatedProductId(request, orderId, eventTime, cancellationToken);
                    if (request.AddonId.HasValue)
                    {
                        await CreatePropertyUpdatedAddonId(request, orderId, eventTime, cancellationToken);
                    }
                    await CreatePropertyUpdatedTotalPrice(request, orderId, totalPrice, eventTime, cancellationToken);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    throw;
                }
            }

            var order = await _orderStateRebuilder.Rebuild(orderId);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResponse
            {
                OrderId = orderId
            };
        }

        private async Task<int> CreateCreatedEvent(DateTimeOffset dateOccurred, CancellationToken cancellationToken)
        {
            var orderId = -1;
            //Lock OrderEvent to get Max(OrderEvent.OrderId) + 1
            orderId = (await _context.OrderEvents.MaxAsync(x => (int?)x.OrderId, cancellationToken) ?? 0) + 1;
            var createdEvent = new OrderEvent
            {
                OrderId = orderId,
                DateOccurred = dateOccurred,
                EventId = (int)OrderEventType.Created
            };
            _context.OrderEvents.Add(createdEvent);
            await _context.SaveChangesAsync(cancellationToken);

            return orderId;
        }

        private async Task<decimal> GetTotalPriceAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var ProductPrice = (await _context.Products.FirstAsync(x => x.Id == request.ProductId, cancellationToken)).Price;
            var AddonPrice = 0M;
            if (request.AddonId.HasValue)
            {
                AddonPrice = (await _context.Addons.FirstAsync(x => x.Id == request.AddonId.Value, cancellationToken)).Price;
            }

            return ProductPrice + AddonPrice;
        }

        private async Task CreatePropertyUpdatedProductId(CreateOrderRequest request, int orderId,
            DateTimeOffset dateOccurred, CancellationToken cancellationToken)
        {
            var product = _context.Products.First(x => x.Id == request.ProductId);
            var payload = JsonConvert.SerializeObject(new PropertyUpdatedPayload(OrderProperty.Product,
                new EventSourcing.Orders.Properties.Product
                {
                    Id = request.ProductId,
                    Name = product.Name,
                    Price = product.Price
                }));
            var createdEvent = new OrderEvent
            {
                OrderId = orderId,
                DateOccurred = dateOccurred,
                EventId = (int)OrderEventType.PropertyUpdated,
                Payload = payload
            };
            _context.OrderEvents.Add(createdEvent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task CreatePropertyUpdatedAddonId(CreateOrderRequest request, int orderId,
            DateTimeOffset dateOccurred, CancellationToken cancellationToken)
        {
            var addon = _context.Addons.First(x => x.Id == request.AddonId);
            var payload = JsonConvert.SerializeObject(new PropertyUpdatedPayload(OrderProperty.Addon,
                new EventSourcing.Orders.Properties.Addon
                {
                    Id = request.AddonId.Value,
                    Name = addon.Name,
                    Price = addon.Price
                }));
            var createdEvent = new OrderEvent
            {
                OrderId = orderId,
                DateOccurred = dateOccurred,
                EventId = (int)OrderEventType.PropertyUpdated,
                Payload = payload
            };
            _context.OrderEvents.Add(createdEvent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task CreatePropertyUpdatedTotalPrice(CreateOrderRequest request, int orderId, decimal totalPrice,
            DateTimeOffset dateOccurred, CancellationToken cancellationToken)
        {
            var payload = JsonConvert.SerializeObject(new PropertyUpdatedPayload(OrderProperty.TotalPrice, totalPrice));
            var createdEvent = new OrderEvent
            {
                OrderId = orderId,
                DateOccurred = dateOccurred,
                EventId = (int)OrderEventType.PropertyUpdated,
                Payload = payload
            };
            _context.OrderEvents.Add(createdEvent);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

/*
        //public int Id { get; set; }
        //public DateTimeOffset DateCreated { get; set; }
        //public DateTimeOffset? DateUpdated { get; set; }
        public int ProductId { get; set; }
        public int? AddonId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
 */
