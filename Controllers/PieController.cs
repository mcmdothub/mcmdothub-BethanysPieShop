using mcmdothub_BethanysPieShop.Models;
using mcmdothub_BethanysPieShop.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace mcmdothub_BethanysPieShop.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult List()
        {
            // instead of passing the data in two ways
            // ViewBag.CurrentCategory = "Cheese cakes";
            // return View(_pieRepository.AllPies);

            // use my constructor of PieListViewModel and pass in the "AllPies" + the current category
            // and return to my view
            PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese cakes");

            return View(piesListViewModel);
        }

        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);

            if(pie == null)
                return NotFound();

            return View(pie);
        }
    }
}
