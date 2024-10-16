using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Camp
    {
        public int CampID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Camp name cannot be longer than 100 characters.")]
        public string CampName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Location cannot be longer than 200 characters.")]
        public string Location { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price per night must be non-negative.")]
        public decimal PricePerNight { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AvailabilityStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EndDateAfterStartDate("AvailabilityStartDate", ErrorMessage = "Availability end date must be after the start date.")]
        public DateTime AvailabilityEndDate { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
    public class EndDateAfterStartDate : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public EndDateAfterStartDate(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = (DateTime)value;
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var startDate = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance);

            if (endDate <= startDate)
            {
                return new ValidationResult("End date must be after start date.");
            }
            return ValidationResult.Success;
        }
    }

}