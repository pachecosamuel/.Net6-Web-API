using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);   
var app = builder.Build();
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);
//builder.Services.AddDbContext<ApplicationDbContext>();

var configuration = app.Configuration;
ProductRepository.Init(configuration);


app.MapPost("/product", (Product product) =>
{
    ProductRepository.AddProduct(product);
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

