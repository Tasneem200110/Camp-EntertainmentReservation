using DAL.Entities;

namespace PL.ViewModels
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public int? CampId { get; set; }
        public Camp? Camp { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
        public List<int>? IdToBeDeleted { get; set; }
        public bool IsDeleted { get; set; } // To track if the image should be removed
    }
}
