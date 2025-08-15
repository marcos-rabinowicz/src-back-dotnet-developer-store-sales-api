using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Sales;

public class SaleItemMap : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> b)
    {
        b.ToTable("sale_items");

        b.HasKey(x => x.Id);

        b.Property<string>("SaleId")
            .HasColumnName("sale_id")
            .IsRequired();

        b.Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        b.Property(x => x.ProductNameSnapshot)
            .HasColumnName("product_name_snapshot")
            .IsRequired();

        b.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        b.Property(x => x.UnitPrice)
            .HasColumnName("unit_price")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        b.Property(x => x.DiscountPercent)
            .HasColumnName("discount_percent")
            .HasColumnType("numeric(5,4)")  // ex.: 0.1000 / 0.2000
            .IsRequired();

        b.Property(x => x.Cancelled)
            .HasColumnName("cancelled")
            .IsRequired();

        // Índice opcional para consultas por venda
        b.HasIndex("SaleId");
    }
}
