using Dapper;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee222Policy")]
    public static IResult Action(int? page, int? rows, QueryAllUserWithClaimName query)
    {
        return Results.Ok(query.Execute(page.Value, rows.Value)); 
    }
}
