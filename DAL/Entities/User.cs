using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class User
{
    public DateTime DateOfCreation = DateTime.Now;
    public int UserID { get; set; }

    [Required(ErrorMessage = "UserName is required")]
    [StringLength(100)]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage =
            "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }

    public string Role { get; set; } // User Or  Admin

    [ForeignKey("Address")] public int? AddressId { get; set; }

    public Address? Address { get; set; }

    public ICollection<Booking> Bookings { get; set; }
}