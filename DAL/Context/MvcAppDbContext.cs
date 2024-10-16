using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class MvcAppDbContext : DbContext
    {
        public MvcAppDbContext(DbContextOptions<MvcAppDbContext> options) : base(options) 
        {

        }
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
                .HasOne(b => b.payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingID);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Camp> Camps { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
