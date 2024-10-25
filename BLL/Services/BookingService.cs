using DAL.Data.Enum;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace BLL.Services
{
    public static class BookingService
    {
        public static List<Booking> GetBookingByCampName(string SelectedCampName, IEnumerable<Booking> bookings)
        {
            return bookings.Where(b => b.camp.CampName == SelectedCampName).ToList();
        }

        public static List<Booking> GetBookingByStatus(string SelectedStatus, IEnumerable<Booking> bookings)
        {
            if (Enum.TryParse(SelectedStatus, out BookingStatus parsedStatus))
            {
                return bookings.Where(c => c.Status == parsedStatus).ToList();
            }
            else
            {
                return new List<Booking>();
            }
        }

        public static IEnumerable<Booking> GetBookingsByTime(string pastOrUpcoming, IEnumerable<Booking> bookings)
        {
            DateTime now = DateTime.Now;

            return pastOrUpcoming.ToLower() switch
            {
                "past" => bookings.Where(b => b.EndDate < now),
                "ongoing" => bookings.Where(b => b.StartDate <= now && b.EndDate >= now),
                "upcoming" => bookings.Where(b => b.StartDate > now),
                _ => throw new ArgumentException("Invalid filter parameter. Use 'past', 'ongoing', or 'upcoming'."),
            };
        }
    }
}
