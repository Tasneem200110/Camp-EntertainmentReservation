using System.ComponentModel.DataAnnotations;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using static DAL.Entities.Booking;

namespace PL.ViewModels
{
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
    }
    
}
