using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.EntityConfigurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(k => k.Id);

            builder.Property(p => p.Code).HasMaxLength(16).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(64).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(512).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.CategoryId).IsRequired();
        }
    }
}