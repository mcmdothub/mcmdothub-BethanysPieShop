using mcmdothub_BethanysPieShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace mcmdothub_BethanysPieShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, IShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }

        // Checkout method is invoked when the page is loaded, when the browser makes the request to order/checkout
        //  [HttpGet] is default if not specified
        public IActionResult Checkout()                     // GET
        {
            return View();
        }

        // this method needs to be called when a POST is received
        // an Order instance "(Order order)" is going to be sent along when we submit the form
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            // first we check that user has items in their shopping cart
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                // ModelState is a byproduct, a side product of model binding
                ModelState.AddModelError("", "Your cart is empty, add some pies first");
            }

            if (ModelState.IsValid)
            {
                // create the order
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();

                // when everything is ok, redirect the user to other view
                return RedirectToAction("CheckoutComplete");
            }

            // if ModelState was not valid we return the same view/ the checkout view
            return View(order);
        }

        // the checkout view
        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order. You'll soon enjoy our delicious pies!";
            return View();
        }
    }
}
