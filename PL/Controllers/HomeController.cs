using System.Diagnostics;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICampRepository _campRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ICampRepository campRepository)
        {
            _logger = logger;
            _campRepository = campRepository;
        }

        public async Task<IActionResult> Index()
        {
            var camps = await _campRepository.GetAll();
            return View(camps);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            // Handle the form submission (e.g., send an email or store the data)

            ViewBag.Message = "Your message has been sent successfully!";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}