using System.ComponentModel.DataAnnotations;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using static DAL.Entities.Booking;

namespace PL.ViewModels
{
    public enum PaymentStatus
    {
        pending,
        completed
    }
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Booking Date is required")]
        [BookingDateValidation(ErrorMessage = "Booking date must be in the future.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EndDateAfterStartDate("StartDate", ErrorMessage = "Availability end date must be after the start date.")]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be non-negative.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Camp Id is required")]
        public int CampID { get; set; }

        [Required]
        public BookingStatus Status { get; set; }
        public int PaymentID { get; set; }

        [Required][DataType(DataType.Date)] public DateTime PaymentDate { get; set; }

        [Required]
        [RegularExpression(@"^(CreditCard|PayPal|BankTransfer)$",
            ErrorMessage = "Payment method must be CreditCard, PayPal, or BankTransfer.")]
        public string PaymentMethod { get; set; } // Allowed values: "CreditCard", "PayPal", "BankTransfer"
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Payment amount must be non-negative.")]
        public decimal Amount { get; set; }

        public PaymentStatus paymentStatus { get; set; }
    }
    
}
