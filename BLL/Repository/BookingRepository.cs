using BLL.Helpers;
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
                booking.Status = BookingStatus.canceled;
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
                booking.Status = BookingStatus.confirmed;
                _context.Bookings.Update(booking );
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
                .Where(b => b.UserID == userId)
                .ToListAsync();
        }

        public async Task<int> GetBookingCount()
        {
            return await _context.Bookings.CountAsync();
        }

        public async Task<int> GetTodayBookingCount()
        {
            var today = DateTime.Now.Date;
            return await _context.Bookings
                                 .Where(b => b.StartDate.Date == today)
                                 .CountAsync();
        }

        public async Task<bool> IsCampAvailableAsync(int campId, DateTime startDate, DateTime endDate)
        {
            var conflictBooking = await _context.Bookings
                                .Where(b => b.CampID == campId && b.StartDate == b.EndDate)
                                .ToListAsync();
            return conflictBooking.Count == 0;
        }


        public async Task<Booking> UpdateBookingAsync(Booking booking)
        {
            var Existing = await _context.Bookings.FindAsync(booking.BookingId);
            if (Existing != null)
            {
                Existing.StartDate = booking.StartDate;
                Existing.EndDate = booking.EndDate;
                Existing.TotalAmount = booking.TotalAmount;
                _context.Bookings.Update(Existing);
                await _context.SaveChangesAsync();
            }

            return Existing;
        }

        

         

        public async Task<List<int>> GetMonthStatistics()
        {
            var currentDate = DateTime.Now;
            var startOfCurrentWeek = DateHelper.StartOfWeek(currentDate); 
            var weekCounts = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                var startOfWeek = startOfCurrentWeek.AddDays(i * 7);
                var endOfWeek = startOfWeek.AddDays(6);
                var count = await _context.Bookings
                    .CountAsync(b => b.StartDate >= startOfWeek && b.StartDate <= endOfWeek);

                weekCounts.Add(count);
            }
            return weekCounts;
        }

        public async Task<List<int>> GetDailyStatistics()
        {
            var currentDate = DateTime.Now;
            var dailyCounts = new List<int>();

            // Loop through the next 7 days
            for (int i = 0; i < 7; i++)
            {
                var currentDay = currentDate.AddDays(i);
                var count = await _context.Bookings
                    .CountAsync(b => currentDay.Date >= b.StartDate.Date && currentDay.Date <= b.EndDate.Date);

                dailyCounts.Add(count);
            }

            return dailyCounts;
        }



    }
}