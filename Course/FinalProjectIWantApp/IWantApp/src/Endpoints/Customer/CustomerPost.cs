namespace IWantApp.Endpoints.Customer;

public class CustomerPost
{
    public static string Template => "/customer";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(CustomerRequest customerRequest, HttpContext http, UserManager<IdentityUser> userManager)
    {
        var user = new IdentityUser { UserName = customerRequest.Email, Email = customerRequest.Email };
        var result = await userManager.CreateAsync(user, customerRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        // User Claims
        var claimResult =
            await userManager.AddClaimAsync(user, new Claim("Name", customerRequest.Name));

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        claimResult =
            await userManager.AddClaimAsync(user, new Claim("Cpf", customerRequest.Cpf));

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        // Final return
        return Results.Created($"/customer/{user.Id}", user.Id);
    }
}
