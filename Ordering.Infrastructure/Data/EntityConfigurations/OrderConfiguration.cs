using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Data.EntityConfigurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(k => k.Id);

            builder.HasIndex(k => k.Id).IsUnique();

            builder.Property(p => p.ProductName)
                .HasMaxLength(64).IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.TotalPrice)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.OrderStatus)
                .HasMaxLength(16).IsRequired();
        }
    }
}