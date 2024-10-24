using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PL.ViewModels;

namespace PL.Controllers
{
    public class AdminController : Controller
    {
        //private readonly IAddressRepository _addressRepository;
        private readonly ICampRepository _campRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        //private readonly IPhotoUploadService _photoService;
        //private readonly IImageRepository _imageRepository;

        public AdminController(ICampRepository campRepository, IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _campRepository = campRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            //_addressRepository = addressRepository;
            //_photoService = photoService;
            //_imageRepository = imageRepository;
        }
        public async Task<IActionResult> Index()
        {
            AdminDashboardViewModel dashboardVM = new AdminDashboardViewModel();
            dashboardVM.CampsCount = await _campRepository.GetCampCount();
            dashboardVM.BookingsCount = await _bookingRepository.GetBookingCount();
            dashboardVM.UsersCount = await _userRepository.GetUsersCount();
            dashboardVM.MonthBookingStatistics = await _bookingRepository.GetMonthStatistics();
            
            //v
            // Prepare the model to send to the view
            //var model = new
            //{
            //    WeekLabels = new[] { "Week 1", "Week 2", "Week 3", "Week 4" }, // Labels for the weeks
            //    //MonthBookingStatistics = weekCounts // Booking counts for each week
            //};



            return View(dashboardVM);
        }
    }
}
