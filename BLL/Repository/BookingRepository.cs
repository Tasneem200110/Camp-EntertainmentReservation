using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MvcAppDbContext _context;

        public BookingRepository(MvcAppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = "Canceled";
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }

            return booking;
        }

        public async Task<Booking> ConfirmBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = "Confirmed";
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }

            return booking;
        }

        public async Task DeleteBookingAsync(int BookingId)
        {
            var booking = await _context.Bookings.FindAsync(BookingId);
            if (booking != null)
            {
                _context.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.camp)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingByCampIdAsync(int CampId)
        {
            return await _context.Bookings
                .Include(b => b.CampID == CampId)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int BookingId)
        {
            return await _context.Bookings
                .Include(b => b.camp)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == BookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.camp)
                .Include(b => b.UserID == userId)
                .ToListAsync();
        }

        public async Task<bool> IsCampAvailableAsync(int campId, DateTime bookingDate)
        {
            var conflictBooking = await _context.Bookings
                .Where(b => b.CampID == campId && b.BookingDate == bookingDate)
                .ToListAsync();
            return conflictBooking.Count == 0;
        }

        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            var Existing = await _context.Bookings.FindAsync(booking.BookingId);
            if (Existing != null)
            {
                Existing.BookingDate = booking.BookingDate;
                Existing.TotalAmount = booking.TotalAmount;
                _context.Bookings.Update(Existing);
                await _context.SaveChangesAsync();
            }

            return Existing;
        }
    }
}