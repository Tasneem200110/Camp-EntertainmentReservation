using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage ="Booking Date is required")]
        [DataType(DataType.Date)]
        [BookingDateValidation(ErrorMessage = "Booking date must be in the future.")]
        public DateTime BookingDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage ="User Id is required")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Camp Id is required")]
        public int CampID { get; set; }
        public User User { get; set; }
        public Camp camp { get; set; }

        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

    }
    public class BookingDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bookingDate = (DateTime)value;
            if (bookingDate <= DateTime.Now)
            {
                return new ValidationResult("Booking date must be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}
