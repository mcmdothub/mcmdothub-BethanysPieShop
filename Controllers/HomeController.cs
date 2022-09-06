using mcmdothub_BethanysPieShop.Models;
using mcmdothub_BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mcmdothub_BethanysPieShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public IActionResult Index()
        {
            var piesOfTheWeek = _pieRepository.PiesOfTheWeek;

            // create a new HomeViewModel with those piesOfTheWeek
            var homeViewModel = new HomeViewModel(piesOfTheWeek);

            return View(homeViewModel);
        }
    }
}
