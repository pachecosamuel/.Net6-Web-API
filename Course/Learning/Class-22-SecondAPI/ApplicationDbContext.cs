using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product>? Products { get; set; }
    public DbSet<Category>? Category { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .Property(p => p.ProductDescription).HasMaxLength(300).IsRequired(false);
        
        builder.Entity<Product>()
            .Property(p => p.ProductName).HasMaxLength(100).IsRequired();

        builder.Entity<Category>()
            .Property(p => p.CategoryName).HasMaxLength(25);

    }
    
}
