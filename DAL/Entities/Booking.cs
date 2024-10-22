using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public enum BookingStatus
    {
        pending,
        confirmed,
        canceled
    }
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage ="Booking Date is required")]
        [BookingDateValidation(ErrorMessage = "Booking date must be in the future.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EndDateAfterStartDate("StartDate", ErrorMessage = "Availability end date must be after the start date.")]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative.")]
        public decimal TotalAmount  { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Camp Id is required")]
        public int CampID { get; set; }

        public User User { get; set; }
        public Camp camp { get; set; }
        public Payment Payment { get; set; }

        [Required]
        public BookingStatus Status { get; set; }


    public class BookingDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bookingDate = (DateTime)value;
            if (bookingDate <= DateTime.Now) return new ValidationResult("Booking date must be in the future.");
            return ValidationResult.Success;
        }
    }
    

}
