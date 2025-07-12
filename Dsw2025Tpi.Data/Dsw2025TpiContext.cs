using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;

public class Dsw2025TpiContext: DbContext
{
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    { 
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //que el sku sea unico
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.sku)
            .IsUnique();

        //que el internalCode sea unico
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.internalCode)
            .IsUnique();
    }
}

