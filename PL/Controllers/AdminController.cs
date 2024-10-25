using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;

namespace PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //private readonly IAddressRepository _addressRepository;
        private readonly ICampRepository _campRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        //private readonly IPhotoUploadService _photoService;
        //private readonly IImageRepository _imageRepository;

        public AdminController(ICampRepository campRepository, IBookingRepository bookingRepository, IUserRepository userRepository, IPaymentRepository paymentRepository)
        {
            _campRepository = campRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            //_addressRepository = addressRepository;
            //_photoService = photoService;
            //_imageRepository = imageRepository;
        }
        public async Task<IActionResult> Index()
        {   
            AdminDashboardViewModel dashboardVM = new AdminDashboardViewModel
            {
                CampsCount = await _campRepository.GetCampCount(),

                BookingsCount = await _bookingRepository.GetBookingCount(),
                TodayBooking = await _bookingRepository.GetTodayBookingCount(),

                UsersCount = await _userRepository.GetUsersCount(),
                TodayNewUsers = await _userRepository.GetTodayNewUsers(),

                MonthBookingStatistics = await _bookingRepository.GetMonthStatistics(),
                WeeklyRevenue = await _paymentRepository.GetWeeklyRevenue(),

                DailyBookingStatistics = await _bookingRepository.GetDailyStatistics(),
                DailyRevenue = await _paymentRepository.GetDailyRevenue(),
                DayLabels = DateHelper.GetUpcoming7DaysLabels(),

                CampNames = await _campRepository.GetCampNames(),
                CampBookings = await _campRepository.GetCampBookingsCount(),

            };
            return View(dashboardVM);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetUsers();
            return View(users);
        }
    }
}
