using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using DAL.Entities;

namespace PL.Controllers
{
    public class CampController : Controller
    {
        private readonly ICampRepository _campRepository;

        public CampController(ICampRepository campRepository)
        {
            _campRepository = campRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Camp> camps = await _campRepository.GetAll();
            return View(camps);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Camp camp = await _campRepository.GetById(id);
            return View(camp);
        }
    }
}
