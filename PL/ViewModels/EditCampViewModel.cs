using DAL.Data.Enum;
using DAL.Entities;

namespace PL.ViewModels
{
    public class EditCampViewModel
    {
        //public int CampID { get; set; }
        public string CampName { get; set; }
        public string Description { get; set; }
        public CampCategory CampCategory { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerNight { get; set; }
        public bool ExisitingAddressFlag { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public DateTime AvailabilityStartDate { get; set; }
        public DateTime AvailabilityEndDate { get; set; }
    }
}
