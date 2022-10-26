using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList();
        var employees = new List<EmployeeResponse>();

        foreach (var u in users)
        {
            var claims = userManager.GetClaimsAsync(u).Result;
            var claimName = claims.FirstOrDefault(c => c.Type == "Name");

            var userName = claimName != null ? claimName.Value : string.Empty;

            employees.Add(new EmployeeResponse(u.Email, userName));
        }
        return Results.Ok(employees);

        /*
         * IConfiguration configuration
         * var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);
        var employees = db.Query<EmployeeResponse>(
            @"select Email, ClaimValue as Name
                from AspNetUsers u inner JOIN AspNetUserClaims c
                on u.Id = c.Id and claimtype = 'Name'"
            );

        return Results.Ok(employees);
         * */
    }
}
