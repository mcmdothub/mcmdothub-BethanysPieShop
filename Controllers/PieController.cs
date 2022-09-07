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

        // Old method
        //public IActionResult List()
        //{
        //    // instead of passing the data in two ways
        //    // ViewBag.CurrentCategory = "Cheese cakes";
        //    // return View(_pieRepository.AllPies);

        //    // use my constructor of PieListViewModel and pass in the "AllPies" + the current category
        //    // and return to my view
        //    PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "All pies");

        //    return View(piesListViewModel);
        //}

        // new List action method that accepts a category
        public ViewResult List(string category)
        {
            IEnumerable<Pie> pies;
            string? currentCategory;

            // check if the category IsNullOrEmpty meaning we want to see AllPies
            if (string.IsNullOrEmpty(category))
            {
                pies = _pieRepository.AllPies.OrderBy(p => p.PieId);
                currentCategory = "All pies";
            }
            else
            {
                // if not empty we go to database => ask for AllPies where the CategoryName == category => order them by PieId
                // and set after the currentCategory to the name of the selected category
                pies = _pieRepository.AllPies.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.PieId);
                currentCategory = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryName == category)?.CategoryName;
            }

            // return the view passing in a PieListViewModel using the pies and the currentCategory
            return View(new PieListViewModel(pies, currentCategory));
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
