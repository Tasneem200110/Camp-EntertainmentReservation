using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

            return View(dashboardVM);
        }
    }
}
