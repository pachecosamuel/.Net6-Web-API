using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ApplicationDbContext dbContext,
        [FromQuery]int page = 1,
        [FromQuery]int row = 10,
        [FromQuery]string orderBy = "name"
        )
    {
        if (row > 10)
            return Results.Problem(title: "Row max value is 10", statusCode: 400);

        var queryBase = dbContext.Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);
        else
            return Results.Problem(title: "Order only can be by order or name");

        var queryFilter = queryBase.Skip((page - 1) * row).Take(row);

        var products = queryFilter.ToList();

        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.ProductDescription, p.Price, p.Id));        
        
        return Results.Ok(results);
    }
}
