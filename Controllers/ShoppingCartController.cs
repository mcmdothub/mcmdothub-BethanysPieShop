using mcmdothub_BethanysPieShop.Models;
using mcmdothub_BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mcmdothub_BethanysPieShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartController(IPieRepository pieRepository, IShoppingCart shoppingCart)
        {
            _pieRepository = pieRepository;
            _shoppingCart = shoppingCart;

        }
        public ViewResult Index()
        {
            // ask the ShoppingCart for all the items
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            // then create a new ShoppingCartViewModel passing in the items + total
            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            // return a view passing in the shoppingCartViewModel
            return View(shoppingCartViewModel);
        }

        // receives the id i want to interact with
        public RedirectToActionResult AddToShoppingCart(int pieId)
        {
            // search in the PieRepository for an existing PieId
            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            // if not null
            if (selectedPie != null)
            {
                // we call the ShoppingCart AddToCart method passing in that pie
                _shoppingCart.AddToCart(selectedPie);
            }
            // we dont return a View but a RedirectToAction method call => redirect us to the Index of the ShoppingCart
            return RedirectToAction("Index");
        }

        // receives the id i want to interact with
        public RedirectToActionResult RemoveFromShoppingCart(int pieId)
        {
            var selectedPie = _pieRepository.AllPies.FirstOrDefault(p => p.PieId == pieId);

            if (selectedPie != null)
            {
                _shoppingCart.RemoveFromCart(selectedPie);
            }
            return RedirectToAction("Index");
        }
    }
}