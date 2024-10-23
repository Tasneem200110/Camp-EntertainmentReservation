using BLL.Helpers;
using BLL.Interfaces;
using BLL.Repository;
using BLL.Services;
using DAL.Context;
using DAL.Data;
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
        builder.Services.AddScoped<ICampRepository, CampRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IPhotoUploadService, PhotoUploadService>();
        builder.Services.AddScoped<IImageRepository, ImageRepository>();
        builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySettings"));
        builder.Services.AddScoped<IUserRepository, UserRepository>();


        var app = builder.Build();

        if (args.Length == 1 && args[0].ToLower() == "seeddata")
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MvcAppDbContext>();
                context.Database.EnsureCreated();
                DataSeeder.SeedCampData(context); // Call your seeding method
            }

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

        app.UseAuthorization();

        app.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}