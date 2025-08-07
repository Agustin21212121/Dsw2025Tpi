using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext : DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Sku)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(p => p.InternalCode)
                  .HasMaxLength(50);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Description)
                  .HasMaxLength(500);

            entity.Property(p => p.CurrentUnitPrice)
                  .HasColumnType("decimal(18,2)");

            entity.Property(p => p.StockQuantity);

            entity.Property(p => p.IsActive)
                  .HasDefaultValue(true);

            entity.HasIndex(p => p.Sku)
                  .IsUnique();

            // Relación uno a muchos: Product → OrderItems
            entity.HasMany(p => p.OrderItems)
                  .WithOne(oi => oi.Product)
                  .HasForeignKey(oi => oi.ProductId);
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);

            entity.HasMany(c => c.Orders)
                  .WithOne(o => o.Customer)
                  .HasForeignKey(o => o.CustomerId);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(200);
            entity.Property(o => o.BillingAddress).IsRequired().HasMaxLength(200);
            entity.Property(o => o.Notes).HasMaxLength(500);
            entity.Property(o => o.Status).IsRequired();

            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.UnitPrice)
                  .HasColumnType("decimal(18,2)");

            entity.Property(oi => oi.Quantity).IsRequired();
        });
    

}
}
