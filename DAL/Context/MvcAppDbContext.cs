using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public class MvcAppDbContext : DbContext
{
    public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Camp> Camps { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlServer("server=.; database=EntertainmentDb; trusted-connection=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserID);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.camp)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CampID);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Payment)
            .WithOne(p => p.Booking)
            .HasForeignKey<Payment>(p => p.BookingID);

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalAmount)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Camp>()
            .Property(c => c.PricePerNight)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18, 2)");
    }
}