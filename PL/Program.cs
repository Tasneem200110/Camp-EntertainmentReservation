using BLL.Helpers;
using BLL.Interfaces;
using BLL.Repository;
using BLL.Services;
using DAL.Context;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        // Define role names
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            // Check if the role exists
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the role if it doesn't exist
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }
    }

    public static async Task SeedRolesAndAdminUserAsync(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager)
    {
        // Create the Admin role
        var adminRole = "Admin";
        var roleExists = await roleManager.RoleExistsAsync(adminRole);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole<int>(adminRole));
        }

        // Create an admin user
        var adminEmail = "admin@gmail.com"; // Change this to your desired email
        var adminPassword = "Admin@123"; // Set a secure password

        var userExists = await userManager.FindByEmailAsync(adminEmail);
        if (userExists == null)
        {
            var adminUser = new User { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                // Assign the admin role to the user
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<MvcAppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddControllersWithViews();

        // Register repositories
        builder.Services.AddScoped<ICampRepository, CampRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPhotoUploadService, PhotoUploadService>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        // Configure Cloudinary settings
        builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySettings"));

        // Configure Identity
        builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 10;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;  // Ensure Email is unique
            // Allow spaces in UserName
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 _";
        })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<MvcAppDbContext>()
        .AddDefaultTokenProviders();



        builder.Services.AddHttpContextAccessor();
        // Create a scope for database seeding
        var app = builder.Build();

        // Seed data if the argument "seeddata" is provided
        if (args.Length == 1 && args[0].ToLower() == "seeddata")
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MvcAppDbContext>();
                context.Database.EnsureCreated();
                DataSeeder.SeedCampData(context); // Call your seeding method
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); // Ensure authentication is enabled
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            await SeedRolesAsync(roleManager);

            await SeedRolesAndAdminUserAsync(roleManager, userManager); // Seed roles and admin user
            //await SeedRolesAsync(roleManager); // Ensure roles are created
        }

        
        app.Run();
    }
}
