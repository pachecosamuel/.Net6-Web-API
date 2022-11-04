using Microsoft.EntityFrameworkCore;
//"Connection": "Server=localhost;Database=Products;User Id=sa;Password=@Sql2022;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES"

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .Property(p => p.ProductDescription).HasMaxLength(300).IsRequired(false);
        
        builder.Entity<Product>()
            .Property(p => p.ProductName).HasMaxLength(100).IsRequired();

        builder.Entity<Category>()
            .Property(p => p.CategoryName).HasMaxLength(25);

    }

    /* Este construtor é para a opção de automatização de conexão, mas me apresenta
    um erro.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    */

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2022;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");

}