using BLL.Interfaces;
using BLL.Repository;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PL.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PL.Controllers
{

    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICampRepository _campRepository;
        private readonly IUserRepository _userRepository;
        private int UserId;
        private readonly IPaymentRepository _paymentRpository;

        public BookingController(IBookingRepository bookingRepository, ICampRepository CampRepository,
                                IUserRepository userRepository, IPaymentRepository paymentRepository) 
        { 
            _bookingRepository = bookingRepository;
            _campRepository = CampRepository;
            _userRepository = userRepository;
            _paymentRpository = paymentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<Booking> bookings;

            int.TryParse(userIdString, out int userId);
            {
                UserId = userId;
                //IdentityUser.Id;
            }
            
            //------------------------------------------------------------for Admin----------------------------------------
            if (User.IsInRole("Admin"))
            {
                bookings = await _bookingRepository.GetAllBookingsAsync();
            }
            else
            {
                bookings = await _bookingRepository.GetBookingByUserIdAsync(UserId);
            }
            return View(bookings);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }


        public async Task<IActionResult> Book(int campId)
        {
            
            //ViewBag.Users = await _userRepository.GetUsers() ?? new List<User>();
            //ViewBag.Camps = await _campRepository.GetAll() ?? new List<Camp>();
            var camp = await _campRepository.GetById(campId);
            var bookingVM = new BookingViewModel
            {
                CampID = camp.CampID,
                AvailabilityStartDate = camp.AvailabilityStartDate,
                AvailabilityEndDate = camp.AvailabilityEndDate
            };
            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookingViewModel bookingVM)
        {
            if (ModelState.IsValid)
            {
                // Fetch the camp and user details
                var camp = await _campRepository.GetById(bookingVM.CampID);
                //var u = User.i
                User user = null;
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userIdString, out int userId))
                {
                    UserId = userId;
                    user = await _userRepository.GetById(UserId);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid user ID.");
                    //return View(bookingVM);
                }

                // Validate if the camp exists
                if (camp == null)
                {
                    ModelState.AddModelError("CampID", "The selected camp does not exist.");
                }

                // Validate if the user exists
                if (user == null)
                {
                    ModelState.AddModelError("UserID", "The selected user does not exist.");
                }

                // Validate dates
                if (bookingVM.StartDate >= camp.AvailabilityEndDate || bookingVM.EndDate >= camp.AvailabilityEndDate)
                {
                    ModelState.AddModelError("", "The venue isn't available in these days");
                    return View(bookingVM);
                }
                Booking booking = new Booking
                {
                    BookingId = bookingVM.BookingId,
                    CampID = bookingVM.CampID,
                    UserID = UserId,
                    StartDate = bookingVM.StartDate,
                    EndDate = bookingVM.EndDate,
                    Status = bookingVM.Status,
                    TotalAmount = bookingVM.TotalAmount

                };

                var price = await _campRepository.GetPriceByCampId(booking.CampID);
                var totalDays = (booking.EndDate - booking.StartDate).Days;
                booking.TotalAmount = totalDays * price * bookingVM.NumberOfPeople;


                await _bookingRepository.AddBookingAsync(booking);
                Payment payment = new Payment
                {
                    BookingID = booking.BookingId,
                    Amount = totalDays * price * bookingVM.NumberOfPeople,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = bookingVM.PaymentMethod,
                    PaymentStatus = PaymentStatus.pending.ToString() // Assuming you're using PaymentStatus enum
                };
                await _paymentRpository.AddPaymentAsync(payment);
                return RedirectToAction("ViewPayment", "Payment", new { bookingId = booking.BookingId });

            }

            // Redirect to Index page after successful booking
            ViewBag.Users = await _userRepository.GetUsers() ?? new List<User>();
            return View(bookingVM);
        }



        //public async Task<IActionResult> Update(int id)
        //{

        //    var booking = await _bookingRepository.GetBookingByIdAsync(id);
        //    var bookingVM = new BookingViewModel
        //    {
        //        CampID = booking.CampID,
        //        AvailabilityStartDate = booking.camp.AvailabilityStartDate,
        //        AvailabilityEndDate = booking.camp.AvailabilityEndDate
        //    };
        //    return View(bookingVM);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int id, BookingViewModel bookingVM)
        //{
        //    Booking booking = new Booking
        //    {
        //        BookingId = bookingVM.BookingId,
        //        CampID = bookingVM.CampID,
        //        UserID = UserId,
        //        StartDate = bookingVM.StartDate,
        //        EndDate = bookingVM.EndDate,
        //        Status = bookingVM.Status,
        //        TotalAmount = bookingVM.TotalAmount

        //    };
        //    if (id != booking.BookingId)
        //    {
        //        return BadRequest();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        await _bookingRepository.UpdateBookingAsync(booking);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(booking);
        //}




        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _bookingRepository.DeleteBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            // Retrieve the booking from the database
            var booking = await _bookingRepository.GetBookingByIdAsync(id);

            if (booking == null)
            {
                return NotFound(); // Return 404 if the booking is not found
            }

            // Check if the booking is already confirmed
            if (booking.Status.Equals(BookingStatus.confirmed))
            {
                TempData["Message"] = "This booking is already confirmed.";
                return RedirectToAction("Index"); // Redirect to the booking list
            }

            booking.Status = BookingStatus.confirmed;
            await _bookingRepository.UpdateBookingAsync(booking);

            TempData["Message"] = "Booking confirmed successfully."; // Set a success message
            return RedirectToAction("Index"); // Redirect to the booking list
        }
    }
}