using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Services;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("default")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking), ServiceLifetime.Transient);
builder.Services.AddIdentity<User, IdentityRole>(op =>
                 {
                     op.Password.RequiredLength = 8;
                     op.Password.RequireNonAlphanumeric = false;
                     op.Lockout.AllowedForNewUsers = true;
                     op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                     op.Lockout.MaxFailedAccessAttempts = 5;
                 })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IMailService>(x => new MailService(builder.Configuration["Mail"], builder.Configuration["AppCode"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapAreaControllerRoute(
         name: "adminarea",
        areaName: "Admin",
        pattern: "admin/{controller=Home}/{action=Index}");

    endpoints.MapAreaControllerRoute(
         name: "visitorarea",
        areaName: "Visitor",
        pattern: "visitor/{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


var container = app.Services.CreateScope();
var userManager = container.ServiceProvider.GetRequiredService<UserManager<User>>();
var roleManager = container.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
if (!await roleManager.RoleExistsAsync("Admin"))
{
    var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
}

var user = await userManager.FindByEmailAsync("admin@admin.com");
if (user is null)
{
    user = new User
    {
        UserName = "admin@admin.com",
        Email = "admin@admin.com",
        Fullname = "Admin",
        Year = 2023,
        EmailConfirmed = true
    };
    var result = await userManager.CreateAsync(user, "Admin2924");
    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
    result = await userManager.AddToRoleAsync(user, "Admin");
}

app.Run();
