using mcmdothub_BethanysPieShop.Models;
using mcmdothub_BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace mcmdothub_BethanysPieShop.Components
{
    // will be a ViewComponent inherit from ViewComponent (built-in class to AspNetCore)
    public class ShoppingCartSummary : ViewComponent
    {
        // we need to have access to the ShoppingCart (for counting how many items are in the shopping cart)
        // this is a scoped instance => so we won't be accessing the database all the time
        // once it'is retrieved for that request, we can use that shopping cart all along while the request is being handled
        private readonly IShoppingCart _shoppingCart;

        // inject to dependency injection to the constructor "(IShoppingCart shoppingCart)"
        public ShoppingCartSummary(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            // use the ShoppingCart and get its items
            var items = new List<ShoppingCartItem>() { new ShoppingCartItem(), new ShoppingCartItem() };

            //var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            // we build again that ShoppingCartViewModel
            var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, _shoppingCart.GetShoppingCartTotal());

            // return the view method passing in the ShoppingCartViewModel
            return View(shoppingCartViewModel);
        }
    }
}
