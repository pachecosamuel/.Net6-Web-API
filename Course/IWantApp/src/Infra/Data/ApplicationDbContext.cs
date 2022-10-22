using Flunt.Notifications;
using IWantApp.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace IWantApp.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) 
        : base(dbContextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.ProductDescription).HasMaxLength(300).IsRequired(false);

        modelBuilder.Entity<Product>()
            .Property(p => p.Name).IsRequired();
        
        modelBuilder.Entity<Category>()
            .Property(c => c.Name).IsRequired();

        modelBuilder.Ignore<Notification>();

        //modelBuilder.Entity<Notification>()
        //  .HasNoKey();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>()
            .HaveMaxLength(100);
    }
}
