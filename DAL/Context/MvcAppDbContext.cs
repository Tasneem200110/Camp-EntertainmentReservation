using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class MvcAppDbContext : IdentityDbContext<User, IdentityRole<int>, int> // Specify int as the primary key
    {
        public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Camp> Camps { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Uncomment and set your connection string if not configured in Startup.cs
            // optionsBuilder.UseSqlServer("server=.; database=EntertainmentDb; trusted-connection=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints for User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure relationships for Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.camp) // Updated to match property naming convention
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CampID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Camp>()
                .Property(c => c.PricePerNight)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)");

            // Configure relationships for Camp and Images
            modelBuilder.Entity<Camp>()
                .HasMany(c => c.Images)
                .WithOne(i => i.Camp)
                .HasForeignKey(i => i.CampId);

            // Uncomment if you want to establish a one-to-one relationship with User and Image
            // modelBuilder.Entity<User>()
            //     .HasOne(u => u.Image)
            //     .WithOne(i => i.User)
            //     .HasForeignKey<Image>(i => i.UserId);
        }
    }
}
