using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/products", (Product product) =>
{
    ProductRepository.AddProduct(product);
    return Results.Created($"/products/{product.Code}", product.Name);
});

app.MapGet("/products/{code}", ([FromRoute] string code) =>
 {
     var product = ProductRepository.GetByCode(code);

     if (product != null)
     {
         return Results.Ok(product);
     }
     return Results.NotFound("falhô");
 });

app.MapPut("/products", (Product product) =>
{
    var productSaved = ProductRepository.GetByCode(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetByCode(code);
    ProductRepository.RemoveProduct(product);
    return Results.Ok();
});

app.Run();

public static class ProductRepository
{
    public static List<Product>? ListProduct { get; set; } = new();

    public static void AddProduct(Product product)
    {
        ListProduct.Add(product);
    }

    public static Product GetByCode(string code)
    {
        return ListProduct.FirstOrDefault(x => x.Code == code);
    }

    public static void RemoveProduct(Product product)
    {
        ListProduct.Remove(product);
        // var product = ProductRepository.GetByCode(code);
        // ListProduct.Remove(product);
    }
}

public class Product
{
    public string? Code { get; set; }
    public string? Name { get; set; }


    public override string? ToString()
    {
        return $"{Name} & {Code}.";
    }
}
