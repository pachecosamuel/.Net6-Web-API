﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
    {
        // User creation
        var user = new IdentityUser { UserName = employeeRequest.Email, Email = employeeRequest.Email };
        var result = userManager.CreateAsync(user, employeeRequest.Password).Result;

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());


        // User Claims
        var claimResult =
            userManager.AddClaimAsync(user, new Claim("Name", employeeRequest.Name)).Result;
        
        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());
        
        claimResult = 
            userManager.AddClaimAsync(user, new Claim("EmployeeCode", employeeRequest.EmployeeCode)).Result;

        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        // Final return
        return Results.Created($"/employees/{user.Id}", user.Id);
    }

}
