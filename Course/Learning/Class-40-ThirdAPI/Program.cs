using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    if (productRequest.Tags != null)
    {
        product.Tags = new List<Tag>();

        foreach (var item in productRequest.Tags)
        {
            product.Tags.Add(new Tag { TagName = item });
        }
    }

    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"product/{product.Id}", product);
});


app.MapGet("/product/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var product = context.Products
    .Include(c => c.Category)
    .Include(t => t.Tags)
    .Where(x => x.Id == id).First();

    if (product == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(product);
});


app.MapPut("/product/{id}", ([FromRoute] int id, ProductRequest productRequest, ApplicationDbContext context) =>
{
    var product = context.Products
    .Include(t => t.Tags)
    .Where(p => p.Id == id).First();

    var category = context.Category.Where(c => c.Id == productRequest.CategoryId).First();

    product.ProductName = productRequest.ProductName;
    product.ProductDescription = productRequest.ProductDescription;
    product.Category = category;
    product.Tags = new List<Tag>();

    if (productRequest.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productRequest.Tags)
        {
            product.Tags.Add(new Tag { TagName = item });
        }
    }

    context.SaveChanges();
    return Results.Ok();
});

app.MapDelete("/product/{id}", ([FromRoute]int id,  ApplicationDbContext context) =>
{
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok("Deleted");
});

app.Run();
