namespace IWantApp.Endpoints.Order;
using IWantApp.Domain.Orders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http, ApplicationDbContext dbContext)
    {
        var customerId = http.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var customerName = http.User.Claims
            .First(c => c.Type == "Name").Value;

        List<Product> productsFound = null;

        if(orderRequest.ProductIds != null && orderRequest.ProductIds.Any())
            productsFound = dbContext.Products.Where(p => orderRequest.ProductIds.Contains(p.Id)).ToList();

        var order = new Order(customerId, customerName, productsFound, orderRequest.DeliveryAdress);

        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());

        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order);



        /* One way without performance
        var products = new List<Product>();
        foreach(var item in orderRequest.ProductIds)
        {
            var product = dbContext.Products.First(p => p.Id == item);
            products.Add(product);
        }
        */
    }
}
