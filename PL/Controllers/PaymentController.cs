using BLL.Interfaces;
using BLL.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PL.ViewModels;
using DAL.Entities;
using System.Security.Claims;

namespace PL.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICampRepository _campRepository;
        private readonly IUserRepository _userRepository;
        private int UserId;

        public PaymentController(ICampRepository campRepository, IPaymentRepository paymentRepository, 
                                IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _campRepository = campRepository;
            _userRepository = userRepository;
        }
        // GET: Payment/Index
        public async Task<IActionResult> Index()
        {
            IEnumerable<Payment> payments;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int.TryParse(userIdString, out int userId);
            {
                UserId = userId;
                //IdentityUser.Id;
            }
            if (User.IsInRole("Admin"))
            {
                payments = await _paymentRepository.GetAllPaymentsAsync();
            }
            else
            {
                payments = await _paymentRepository.GetPaymentsByUserAsync(UserId);
            }
            // Return the view with the list of payments
            return View(payments);
        }
        // GET: Payment/View/{bookingId}
        public async Task<IActionResult> ViewPayment(int bookingId)
        {
            // Get payment information by booking ID
            var payment = await _paymentRepository.GetPaymentByBookingId(bookingId);

            if (payment == null)
            {
                return NotFound("No payment found for this booking.");
            }

            // Assuming one payment per booking for simplicity
            var booking = await _bookingRepository.GetBookingByIdAsync(payment.BookingID);

            // Fetch user information
            var user = await _userRepository.GetById(booking.UserID);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Send the payment details to the view
            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int paymentId)
        {
            // Fetch the payment by paymentId
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);

            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            // Update the payment status to Completed
            payment.PaymentStatus = PaymentStatus.completed.ToString();

            // Update the payment in the repository
            await _paymentRepository.UpdatePaymentAsync(payment);

            // Optionally, you can redirect to a different page or return a success message
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PaymentByBookingId(int BookingId)
        {
            // Fetch all payments from the repository
            var payment = await _paymentRepository.GetPaymentByBookingId(BookingId);

            // Return the view with the list of payments
            return View(payment);
        }

    }
}
