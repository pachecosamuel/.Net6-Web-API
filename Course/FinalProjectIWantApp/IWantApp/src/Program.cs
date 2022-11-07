using IWantApp.Endpoints.Customer;
using IWantApp.Endpoints.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionString:IWantDb"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Depois da configuração do AddAuthorization ...
//O swagger não está mais funcionando por questões ...
//de habilitação de usuário
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("EmployeePolicy", p =>
    p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));

    options.AddPolicy("Employee222Policy", p =>
    p.RequireAuthenticatedUser().RequireClaim("EmployeeCode", "222"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtBearerTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtBearerTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerTokenSettings:SecretKey"]))
    };
});

builder.Services.AddScoped<QueryAllUserWithClaimName>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext http) =>
{
    return Results.Problem(title: "An error ocurred", statusCode: 500);
});


app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);

app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);

app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);

app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle);

app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle);

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handle);

app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);

app.MapMethods(ProductGetShowCase.Template, ProductGetShowCase.Methods, ProductGetShowCase.Handle);

app.MapMethods(ProductGetById.Template, ProductGetById.Methods, ProductGetById.Handle);

app.MapMethods(CustomerPost.Template, CustomerPost.Methods, CustomerPost.Handle);

app.Run();

