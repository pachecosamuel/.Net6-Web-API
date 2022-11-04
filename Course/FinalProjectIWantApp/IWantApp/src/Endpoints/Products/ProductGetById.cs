using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Endpoints.Products;

public class ProductGetById
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id,ApplicationDbContext dbContext)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return Results.BadRequest();

        return Results.Ok(product);
    }
}
