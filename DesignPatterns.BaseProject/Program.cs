using DesignPatterns.BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    identityDbContext.Database.Migrate();

    if (!userManager.Users.Any())
    {
        userManager.CreateAsync(new AppUser { UserName = "user1", Email = "user1@test.com" }, "Aa1234*").Wait();
        userManager.CreateAsync(new AppUser { UserName = "user2", Email = "user2@test.com" }, "Aa1234*").Wait();
        userManager.CreateAsync(new AppUser { UserName = "user3", Email = "user3@test.com" }, "Aa1234*").Wait();
        userManager.CreateAsync(new AppUser { UserName = "user4", Email = "user4@test.com" }, "Aa1234*").Wait();
        userManager.CreateAsync(new AppUser { UserName = "user5", Email = "user5@test.com" }, "Aa1234*").Wait();
    }
}

app.Run();
