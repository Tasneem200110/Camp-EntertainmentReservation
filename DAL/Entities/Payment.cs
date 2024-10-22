using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }

        [Required][DataType(DataType.Date)] public DateTime PaymentDate { get; set; }

        [Required]
        [RegularExpression(@"^(CreditCard|PayPal|BankTransfer)$",
            ErrorMessage = "Payment method must be CreditCard, PayPal, or BankTransfer.")]
        public string PaymentMethod { get; set; } // Allowed values: "CreditCard", "PayPal", "BankTransfer"

        [Required]
        [RegularExpression(@"^(Paid|Pending|Failed)$", ErrorMessage = "Payment status must be Paid, Pending, or Failed.")]
        public string PaymentStatus { get; set; } // Allowed values: "Paid", "Pending", "Failed"

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Payment amount must be non-negative.")]
        public decimal Amount { get; set; }

        [Required] public int BookingID { get; set; } // Foreign key to Booking

        // Navigation property
        public Booking Booking { get; set; }
    }
}