using DAL.Entities;

namespace PL.ViewModels
{
    public class BookingListViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; } = Enumerable.Empty<Booking>();

        public IEnumerable<string> CampNames { get; set; }
        public IEnumerable<string> BookingStatus { get; set; }
        //public IEnumerable<string> Cities { get; set; }
        //public IEnumerable<string> Districts { get; set; }

        public string? SelectedCampName { get; set; }
        public string? SelectedStatus { get; set; }
        public string? PastOrUpcoming  { get; set; }
        //public string SelectedDistrict { get; set; }

    }
}
