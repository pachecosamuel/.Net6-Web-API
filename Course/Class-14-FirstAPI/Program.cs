using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//1°
app.MapGet("/", () => "This is my first API in .Net platform!\nI'm so proud of me.\nThird Line!");

//2°
app.MapPost("/", () => new { Name = "Samuel Pacheco", Age = 24, Salary = 19562.20 });

//3°
app.MapGet("/Header", (HttpResponse response) =>
{
    response.Headers.Add("MyHeader", "Samuel Millionare");
    return new { Name = "Samuel Pacheco", Age = 24, Salary = 19562.20 };
});

//4°
app.MapPost("/saveproduct", (Product product) =>
{
    return product.Code + " & " + product.Name;
});

//5°
// api.app.com/users?datastart={date}&dateend={date}
app.MapGet("/getproduct", ([FromQuery] string startDate, [FromQuery] string endDate) =>
{
    return $"Start: {startDate} & Finish: {endDate}";
});

//6°
// api.app.com/user/{code}
app.MapGet("/getproduct/{code}", ([FromRoute] string code) =>
{
    return code;
});

//7°
app.MapGet("/getproductbyheader", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});

app.Run();

public static class ProductRepository
{
    public static List<Product> ListProducts { get; set; } = new();

    public static void AddProduct(Product product)
    {
        ListProducts.Add(product);
    }

    public static Product GetByCode(string code)
    {
        return ListProducts.First(p => p.Code == code);
    }
}

public class Product
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}
