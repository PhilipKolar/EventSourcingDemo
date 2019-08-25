using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventSourcingDemo.Data.DomainConfig
{
    public class OrderEventConfig : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateOccurred)
                .IsRequired();
            builder.Property(x => x.EventId)
                .IsRequired();
        }
    }
}
