using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    { 
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PRODUCT: SKU e internalCode únicos
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.sku)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.internalCode)
            .IsUnique();

        // PRODUCT: precio con precisión
        modelBuilder.Entity<Product>()
            .Property(p => p.currentUnitPrice)
            .HasPrecision(18, 2);

        // ORDER: totalAmount con precisión
        modelBuilder.Entity<Order>()
            .Property(o => o.totalAmount)
            .HasPrecision(18, 2);

        // ORDERITEM: unitPrice y subtotal con precisión
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.unitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.subtotal)
            .HasPrecision(18, 2);

        // Relación: Order → OrderItems (1 a muchos)
        modelBuilder.Entity<Order>()
            .HasMany(o => o.orderItems)
            .WithOne(oi => oi.order)
            .HasForeignKey(oi => oi.orderId);

        // Relación: OrderItem → Product (muchos a uno)
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.product)
            .WithMany()
            .HasForeignKey(oi => oi.productId);

        // Relación: Order → Customer (muchos a uno)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.customerId);

        // Opcional: si OrderStatus es una entidad (no un enum)
        // modelBuilder.Entity<Order>()
        //     .HasOne(o => o.OrderStatus)
        //     .WithMany()
        //     .HasForeignKey(o => o.OrderStatusId);
    }

}

