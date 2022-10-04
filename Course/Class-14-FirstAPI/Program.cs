var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "This is my first API in .Net platform!\nI'm so proud of me.\nThird Line!");
app.MapPost("/", () => new { Name = "Samuel Pacheco", Age = 24, Salary = 19562.20 });
app.MapGet("/Header", (HttpResponse response) =>
{
    response.Headers.Add("MyHeader", "Samuel Millionare");
    return new { Name = "Samuel Pacheco", Age = 24, Salary = 19562.20 };
});
app.Run();
