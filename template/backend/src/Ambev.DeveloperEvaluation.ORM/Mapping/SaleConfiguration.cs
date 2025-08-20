using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> b)
    {
        b.ToTable("Sales");
        b.HasKey(x => x.Id);

        b.Property(x => x.SaleNumber).HasMaxLength(64).IsRequired();
        b.Property(x => x.Date).IsRequired();
        b.Property(x => x.Status).HasConversion<int>().IsRequired();

        var cust = b.OwnsOne(s => s.Customer, c =>
        {
            c.Property(p => p.Id).HasColumnName("CustomerId").IsRequired();
            c.Property(p => p.Name).HasColumnName("CustomerName").HasMaxLength(128).IsRequired();
        });

        var branch = b.OwnsOne(s => s.Branch, c =>
        {
            c.Property(p => p.Id).HasColumnName("BranchId").IsRequired();
            c.Property(p => p.Name).HasColumnName("BranchName").HasMaxLength(128).IsRequired();
        });

        b.OwnsMany(s => s.Items, nav =>
        {
            nav.ToTable("SaleItems");
            nav.WithOwner().HasForeignKey("SaleId");

            nav.HasKey(i => i.Id);
            nav.Property(i => i.Id).ValueGeneratedNever();

            nav.OwnsOne(i => i.Product, p =>
            {
                p.Property(x => x.Id).HasColumnName("ProductId").IsRequired();
                p.Property(x => x.Name).HasColumnName("ProductName").HasMaxLength(128).IsRequired();
            });

            nav.Property(i => i.Quantity).IsRequired();
            nav.Property(i => i.UnitPrice).HasColumnType("numeric(18,2)").IsRequired();
            nav.Property(i => i.DiscountPercent).HasColumnType("numeric(5,2)").IsRequired();
            nav.Property(i => i.IsCancelled).IsRequired();

            nav.Ignore(i => i.LineTotal);
        });

        b.Navigation(s => s.Items).UsePropertyAccessMode(PropertyAccessMode.Field);

        b.Ignore(x => x.TotalAmount);
        b.Ignore(x => x.DomainEvents);
    }
}
