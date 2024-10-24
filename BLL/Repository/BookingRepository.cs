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

        //public async Task<List<int>> GetMonthStatistics()
        //{
        //    // Get the current date
        //    var currentDate = DateTime.Now;

        //    // Get the first day of the current month
        //    var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        //    // Get the last day of the current month
        //    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        //    // Get booking counts for each of the four weeks
        //    var weekCounts = new List<int>();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        var startOfWeek = startOfMonth.AddDays(i * 7);
        //        var endOfWeek = startOfWeek.AddDays(6);
        //        if (endOfWeek > endOfMonth) endOfWeek = endOfMonth; // Ensure we don't go past the end of the month

        //        var count = _context.Bookings.Count(b => b.StartDate >= startOfWeek && b.StartDate <= endOfWeek);

        //        weekCounts.Add(count);
        //    }
        //    return weekCounts;
        //}

         DateTime StartOfWeek(DateTime date)
        {
            // Get the start of the week (Sunday)
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-diff).Date;
        }

        public async Task<List<int>> GetMonthStatistics()
        {
            // Get the current date
            var currentDate = DateTime.Now;

            // Get the start date for the current week (Sunday)
            var startOfCurrentWeek = StartOfWeek(currentDate); // You may need a helper method for this

            // Initialize a list to hold booking counts for the current week and the next three weeks
            var weekCounts = new List<int>();

            // Iterate for the next four weeks (current week + 3 upcoming weeks)
            for (int i = 0; i < 4; i++)
            {
                var startOfWeek = startOfCurrentWeek.AddDays(i * 7);
                var endOfWeek = startOfWeek.AddDays(6); // End of the week is 6 days after the start

                // Count bookings where StartDate is within the current week and the next three weeks
                var count = await _context.Bookings
                    .CountAsync(b => b.StartDate >= startOfWeek && b.StartDate <= endOfWeek);

                weekCounts.Add(count);
            }

            return weekCounts;
        }

    }
}