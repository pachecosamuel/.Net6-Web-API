using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Criar entidade
//Criar repository
//Criar Crud

app.MapPost("/product", (Product product) =>
{
    ProductRepository.AddProduct(product);
    return Results.Created($"/product/{product.ProductId}", product);
});

app.MapGet("/product/{id}", ([FromRoute] string id) =>
{
    var product = ProductRepository.GetById(id);

    if (product != null)
        return Results.Ok(product);

    return Results.NotFound();
});

app.MapGet("/configuration/database", (IConfiguration configuration) =>
{
    return Results.Ok(configuration["database:connection"]);
});

app.MapPut("/product", (Product product) =>
{
    var localProduct = ProductRepository.GetById(product.ProductId);

    if (localProduct == null)
        return Results.NotFound();

    localProduct.ProductName = product.ProductName;

    return Results.Ok(localProduct.ProductName);
});

app.MapDelete("/product/{id}", ([FromRoute] string id) =>
{
    var localProduct = ProductRepository.GetById(id);
    ProductRepository.RemoveProduct(localProduct);

    return Results.Ok();
});

app.Run();

public static class ProductRepository
{
    public static List<Product> ProductList { get; set; } = new();

    public static void AddProduct(Product product)
    {
        ProductList.Add(product);
    }

    public static void RemoveProduct(Product product)
    {
        ProductList.Remove(product);
    }

    public static Product GetById(string id)
    {
        return ProductList.FirstOrDefault(x => x.ProductId == id);
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
