using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapPost("/product", (Product product) =>
{
    ProductRepository.AddProduct(product);
    return Results.Created($"product/{product.ProductId}", product);
});

app.MapGet("/product/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetByCode(code);
    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

if (app.Environment.IsStaging())
{
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
    {
        return Results.Ok($"{configuration["database:connection"]} + {configuration["database:port"]}");
    });
}

app.MapGet("/configuration/database", (IConfiguration configuration) =>
   {
       return Results.Ok($"{configuration["database:connection"]} + {configuration["database:port"]}");
   });

app.MapPut("/product", (Product product) =>
{
    var productSaved = ProductRepository.GetByCode(product.ProductId);
    productSaved.ProductName = product.ProductName;
    return Results.Ok();
});

app.MapDelete("/product/{code}", (string code) =>
{
    var product = ProductRepository.GetByCode(code);

    ProductRepository.RemoveProduct(product);
    return Results.Ok("Deleted");
});

app.Run();

public static class ProductRepository
{
    public static List<Product> ProductList { get; set; } = new();

    public static void Init(IConfiguration configuration)
    {
        var products = configuration.GetSection("Products").Get<List<Product>>();
        ProductList = products;
    }

    public static void AddProduct(Product product)
    {
        ProductList.Add(product);
    }

    public static void RemoveProduct(Product product)
    {
        ProductList.Remove(product);
    }

    public static Product GetByCode(string code)
    {
        return ProductList.FirstOrDefault(x => x.ProductId == code);
    }
}

public class Category
{
    public int CategoryId { get; set; }
    public String CategoryName { get; set; }
}

public class Tag
{
    public int TagId { get; set; }
    public string TagName { get; set; }

    public int ProductId { get; set; }
}

public class Product
{
    public string? ProductName { get; set; }
    public string? ProductId { get; set; }

    public string? Description { get; set; }

    public Category Category { get; set; }

    public List<Tag> Tags { get; set; } = new();

    public override string? ToString()
    {
        return $"{ProductName} & {ProductId}";
    }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Product>? Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder1)
    {
        builder1.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(250).IsRequired(false);

        builder1.Entity<Product>()
            .Property(p => p.ProductName).HasMaxLength(100).IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2022;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");

}