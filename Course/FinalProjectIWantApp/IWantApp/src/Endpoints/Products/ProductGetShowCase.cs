using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Products;

public class ProductGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        [FromQuery]int? page,
        [FromQuery] int? row,
        [FromQuery] string? orderBy, 
        ApplicationDbContext dbContext)
    {
        if (page == null)
            page = 1;
        if (row == null)
            row = 10;
        if (string.IsNullOrEmpty(orderBy))
            orderBy = "name";

        var queryBase = dbContext.Products
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else
            queryBase = queryBase.OrderBy(p => p.Price);

        var queryFilter = queryBase.Skip((page.Value - 1) * row.Value).Take(row.Value);

        var products = queryFilter.ToList();

        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.ProductDescription, p.Price, p.Id));        
        
        return Results.Ok(results);
    }
}
