using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();
var app = builder.Build();
// builder.Services.AddDbContext<ApplicationDbContext>();

app.MapPost("/product", (ProductRequest productRequest, ApplicationDbContext context) =>
{
    var category = context.Category.Where(c => c.Id == productRequest.CategoryId).First();

    var product = new Product
    {
        ProductName = productRequest.ProductName,
        ProductDescription = productRequest.ProductDescription,
        Category = category
    };

    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"product/{product.Id}", product);
});

app.MapGet("/product/{id}", ([FromRoute] int id) =>
{
    var product = ProductRepository.GetById(id);
    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});

app.MapPut("/product", (Product product) =>
{
    var productSaved = ProductRepository.GetById(product.Id);
    productSaved.ProductName = product.ProductName;
    return Results.Ok();
});

app.MapDelete("/product/{id}", (int id) =>
{
    var product = ProductRepository.GetById(id);

    ProductRepository.RemoveProduct(product);
    return Results.Ok("Deleted");
});

app.Run();
