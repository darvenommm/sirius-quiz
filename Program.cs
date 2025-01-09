using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using Quiz.Environments;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITestService, TestService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(EnvironmentsSettings.GetDatabaseSettings())
);

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<AppDbContext>();

    var roleExist = await roleManager.RoleExistsAsync("Admin");
    if (!roleExist)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var adminUser = await userManager.FindByEmailAsync(EnvironmentsSettings.GetAdminEmail());
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = EnvironmentsSettings.GetAdminUsername(),
            Email = EnvironmentsSettings.GetAdminEmail(),
        };

        var result = await userManager.CreateAsync(adminUser, EnvironmentsSettings.GetAdminPassword());

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tests}/{action=Index}/{id?}");

app.Run();
