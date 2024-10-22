using DAL.Data.Enum;
using DAL.Entities;

namespace PL.ViewModels
{
    public class ListCampViewModel
    {
        public IEnumerable<Camp> Camps { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Governments { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> Districts { get; set; }

        public string? SelectedCategory { get; set; }
        public string SelectedGovernment { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedDistrict { get; set; }
    }

}
