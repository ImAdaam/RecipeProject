using RecipeProject.DbContext;
using RecipeProject.Services;
using Microsoft.EntityFrameworkCore;
using RecipeProject.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using RecipeProject.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using RecipeProject.Policy;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<RecipeDbContext>(options =>
{
    var dbBuilder = options.UseSqlServer(@"Server=(local);DataBase=RecipeProject;Trusted_Connection=true;MultipleActiveResultSets=true");
    if (builder.Environment.IsDevelopment())
    {
        dbBuilder.EnableSensitiveDataLogging();
    }
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IAllergenService, AllergenService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IIngredientCategoryService, IngredientCategoryService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options));
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options => builder.Configuration.Bind("CookieSettings", options));


builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<RecipeDbContext>()
                .AddDefaultTokenProviders();
/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireActiveUser", policy =>
    {
        policy.Requirements.Add(new ActiveUserPolicy());
    });
});

builder.Services.AddTransient<IAuthorizationHandler, RequireActiveUserHandler>();
*/

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Logging.ClearProviders();
//builder.Logging.AddColorConsoleLogger();

var app = builder.Build();

//app.UseMiddleware<Guid>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.MapRazorPages();

app.Run();
