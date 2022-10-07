var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapPost("/product", (Product product) =>
{
    ProductRepository.AddProduct(product);
    return Results.Created($"product/{product.ProductId}", product);
});

app.MapGet("/product/{code}", (string code) =>
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

public class Product
{
    public string? ProductName { get; set; }
    public string? ProductId { get; set; }

    public override string? ToString()
    {
        return $"{ProductName} & {ProductId}";
    }
}