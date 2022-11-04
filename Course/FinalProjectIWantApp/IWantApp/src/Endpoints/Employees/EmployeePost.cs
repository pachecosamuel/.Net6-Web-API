using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(EmployeeRequest employeeRequest, HttpContext http,UserManager<IdentityUser> userManager)
    {
        // User creation
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
        var result = await userManager.CreateAsync(user, employeeRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        // User Claims
        var claimResult =
            await userManager.AddClaimAsync(user, new Claim("Name", employeeRequest.Name));
        
        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());
        
        claimResult =
            await userManager.AddClaimAsync(user, new Claim("EmployeeCode", employeeRequest.EmployeeCode));

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        claimResult =
            await userManager.AddClaimAsync(user, new Claim("CreatedBy", userId));

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        // Final return
        return Results.Created($"/employees/{user.Id}", user.Id);
    }

}
