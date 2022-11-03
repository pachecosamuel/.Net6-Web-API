﻿namespace IWantApp.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext dbContext)
    {
        var products = dbContext.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var results = products.Select(p => new ProductResponse(p.Name, p.Category.Name, p.ProductDescription, p.HasStock, p.Active, p.Id));
        return Results.Ok(results);
    }
}