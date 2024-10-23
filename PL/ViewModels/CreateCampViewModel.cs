using DAL.Data.Enum;
using DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{

    public class CreateCampViewModel
    {
        public int CampID { get; set; }
        public string CampName { get; set; }
        public string Description { get; set; }
        public CampCategory CampCategory { get; set; }
        public bool ExisitingAddressFlag { get; set; }
        public List<IFormFile>? ImagesUrls { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerNight { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        [DataType(DataType.Date)] public DateTime AvailabilityStartDate { get; set; }
        [DataType(DataType.Date)] public DateTime AvailabilityEndDate { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}