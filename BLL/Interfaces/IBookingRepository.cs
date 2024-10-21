using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddBookingAsync(Booking booking);
        Task<Booking> GetBookingByIdAsync(int BookingId);
        Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingByCampIdAsync(int CampId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int BookingId);
        Task<Booking> ConfirmBookingAsync(int bookingId);
        Task<Booking> CancelBookingAsync(int bookingId);
        Task<bool> IsCampAvailableAsync(int campId, DateTime dateTime);



    }
}
