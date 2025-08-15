using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Sales;

public class SaleMap : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> b)
    {
        b.ToTable("sales");

        b.HasKey(x => x.Id);

        b.Property(x => x.SaleNumber)
            .HasColumnName("sale_number")
            .IsRequired();

        b.HasIndex(x => x.SaleNumber)
            .IsUnique();

        b.Property(x => x.SaleDate)
            .HasColumnName("sale_date")
            .IsRequired();

        b.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        b.Property(x => x.CustomerNameSnapshot)
            .HasColumnName("customer_name_snapshot")
            .IsRequired();

        b.Property(x => x.BranchId)
            .HasColumnName("branch_id")
            .IsRequired();

        b.Property(x => x.BranchNameSnapshot)
            .HasColumnName("branch_name_snapshot")
            .IsRequired();

        b.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        // Relacionamento 1:N com backing field _items
        b.HasMany(s => s.Items)
         .WithOne()
         .HasForeignKey("SaleId")
         .OnDelete(DeleteBehavior.Cascade);

        // Usa o backing field para acesso
        var nav = b.Metadata.FindNavigation(nameof(Sale.Items));
        nav?.SetField("_items");
        nav?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
