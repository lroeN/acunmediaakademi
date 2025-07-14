using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models.Entity;

namespace InventoryManagement.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Product> Products { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Inventory inventory)
            {
                if (entry.State == EntityState.Added)
                    inventory.CreatedAt = now;

                inventory.UpdatedAt = now;
            }

            if (entry.Entity is Product product)
            {
                if (entry.State == EntityState.Added)
                    product.CreatedAt = now;

                product.UpdatedAt = now;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        
        builder.Entity<Inventory>()
            .HasOne(i => i.User)
            .WithMany(u => u.Inventories)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.Entity<Product>()
            .HasOne(p => p.Inventory)
            .WithMany(i => i.Products)
            .HasForeignKey(p => p.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}