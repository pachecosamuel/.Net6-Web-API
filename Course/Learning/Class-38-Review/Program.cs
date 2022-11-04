using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);


app.MapPost("/product", (ProductRequest productRequest, ApplicationDbContext context) =>
{
    var category = context.Category?.Where(x => x.CategoryId == productRequest.CategoryId).FirstOrDefault();

    var product = new Product
    {
        ProductId = productRequest.ProductId,
        ProductName = productRequest.ProductName,
        Description = productRequest.Description,
        Category = category,
    };

    context.Products.Add(product);
    return Results.Created($"/{product.ProductId}", product);
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

app.MapGet("/configuration", (IConfiguration configuration) =>
{
    return Results.Ok($"{configuration["database:connection"] + ", " + "Port: " + configuration["database:port"]}");
});

app.MapPut("/product", (Product product) =>
{
    var localProduct = ProductRepository.GetById(product.ProductId);
    localProduct.ProductName = product.ProductName;

    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(localProduct);
});

app.MapDelete("/product/{id}", ([FromRoute] int id) =>
{
    var product = ProductRepository.GetById(id);

    if (product == null)
    {
        return Results.NotFound();
    }

    ProductRepository.RemoveProduct(product);
    return Results.Ok(product);
});

app.Run();
