using BLL.Interfaces;
using BLL.Repository;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _BookingRepository;
        private readonly ICampRepository _campRepository;
        private readonly IUserRepository _userRepository;

        public BookingController(IBookingRepository bookingRepository, ICampRepository CampRepository, IUserRepository userRepository) 
        { 
            _BookingRepository = bookingRepository;
            _campRepository = CampRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _BookingRepository.GetAllBookingsAsync();
            return View(bookings);
        }


        public async Task<IActionResult> Book(int campId)
        {
            var camp = await _campRepository.GetById(campId);
            if (camp == null)
            {
                return NotFound();
            }
            var booking = new Booking
            {
                CampID = campId,
                BookingDate = DateTime.Now


            };
            return View(booking);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var camp = await _campRepository.GetById(booking.CampID);
                if (camp == null)
                {
                    return NotFound();
                }
                var user = await _userRepository.GetById(booking.UserID);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid User Id");
                    return View(booking);
                }
                bool IsAvailable = await _BookingRepository.IsCampAvailableAsync(booking.CampID, booking.BookingDate);
                if(!IsAvailable)
                {
                    ModelState.AddModelError("", "The Camp is not aAvailable for the selected dates");
                    return View(booking);
                }
                await _BookingRepository.AddBookingAsync(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        public async Task<IActionResult> update(int id)
        {
            var booking = await _BookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _BookingRepository.UpdateBookingAsync(booking);
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _BookingRepository.CancelBookingAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _BookingRepository.ConfirmBookingAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _BookingRepository.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _BookingRepository.DeleteBookingAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
