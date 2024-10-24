using BLL.Interfaces;
using BLL.Repository;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using PL.ViewModels;

namespace PL.Controllers
{

    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICampRepository _campRepository;
        private readonly IUserRepository _userRepository;
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
            var bookings = await _bookingRepository.GetAllBookingsAsync();
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
            
            ViewBag.Users = await _userRepository.GetUsers() ?? new List<User>();
            //ViewBag.Camps = await _campRepository.GetAll() ?? new List<Camp>();
            var camp = await _campRepository.GetById(campId);
            var bookingVM = new BookingViewModel
            {
                CampID = campId,
                //Camp = camp,
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
                var user = await _userRepository.GetById(bookingVM.UserID);

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
                    UserID = bookingVM.UserID,
                    StartDate = bookingVM.StartDate,
                    EndDate = bookingVM.EndDate,
                    Status = bookingVM.Status,
                    TotalAmount = bookingVM.TotalAmount

                };

                var price = await _campRepository.GetPriceByCampId(booking.CampID);
                var totalDays = (booking.EndDate - booking.StartDate).Days;
                booking.TotalAmount = totalDays * price;


                await _bookingRepository.AddBookingAsync(booking);
                Payment payment = new Payment
                {
                    BookingID = booking.BookingId,
                    Amount = totalDays * price,
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



        public async Task<IActionResult> Update(int id)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BookingViewModel bookingVM)
        {
            Booking booking = new Booking
            {
                BookingId = bookingVM.BookingId,
                CampID = bookingVM.CampID,
                UserID = bookingVM.UserID,
                StartDate = bookingVM.StartDate,
                EndDate = bookingVM.EndDate,
                Status = bookingVM.Status,
                TotalAmount = bookingVM.TotalAmount

            };
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _bookingRepository.UpdateBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }




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

    }
}