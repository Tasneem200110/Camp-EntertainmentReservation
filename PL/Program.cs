using BLL.Helpers;
using BLL.Interfaces;
using BLL.Repository;
using BLL.Services;
using DAL.Context;
using DAL.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
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
        builder.Services.AddScoped<IPhotoUploadService, PhotoUploadService>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        // Configure Cloudinary settings
        builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySettings"));

        // Configure Identity
        builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;  // Ensure Email is unique
        })
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

        app.Run();
    }
}
