using Microsoft.EntityFrameworkCore;

namespace mcmdothub_BethanysPieShop.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        public string? ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        private ShoppingCart(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        // we call this method in "Program.cs"
        // GetCart method we did not have in the interface beacause is a static method  => will retun a fully created ShoppingCart
        // pass in the service collection "IServiceProvider" and injected in the GetCart method "(IServiceProvider services)"=> 
        // when the user visit the site this code is going to run and is going to check if there is already an ID called CartId available for that user
        // if not we will create a new GUID and restore that value as the CartId
        // when uesr is returning we'll be able to find the existing CartId and we'll use that
        public static ShoppingCart GetCart(IServiceProvider services)
        {
            // using the session (session is available through the dependency injection system)
            // sessions will give the ability to store information about a returning user
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            // then we try to get access to the DbContext
            BethanysPieShopDbContext context = services.GetService<BethanysPieShopDbContext>() ?? throw new Exception("Error initializing");

            // then we're going to check based on the session if there is already a value called CartId for the incoming user
            // if not then we create a new ID, a new GUID => "Guid.NewGuid().ToString();"
            // if there is one we will take that value from the session => "session?.GetString("CartId")"
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            // we then set the value of the CartId (in anycase if we find it or not, we just add it again to the session)
            session?.SetString("CartId", cartId);

            // finally return the ShoppingCart passing in the DbContext and passing in the ShoppingCartId as the CartId that was either now generated or returned from session
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        // AddToCart method receives the pie
        public void AddToCart(Pie pie)
        {
            // first we check in the ShoppingCartItems in the database to see for the given ShoppingCartId if there is already a pie with that ID
            var shoppingCartItem =
                    _bethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            // IF NOT then
            if (shoppingCartItem == null)
            {
                // will create a new ShoppingCartItem 
                // setting the amount to 1
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };

                // we store it using DbContext.ShoppingCartItems
                _bethanysPieShopDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                // else we find that pie already for the given ShoppingCartId => we increase the amount
                shoppingCartItem.Amount++;
            }
            // any cases we save the changes using the DbContext 
            _bethanysPieShopDbContext.SaveChanges();
        }

        public int RemoveFromCart(Pie pie)
        {
            // check if we find the item already
            var shoppingCartItem =
                    _bethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(
                        s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            // if not null
            if (shoppingCartItem != null)
            {
                // and amount > 1
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;                      // we deduct one from the ShoppingCartItem
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    // if is the last item then we remove the item altogether 
                    _bethanysPieShopDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            // we save it
            _bethanysPieShopDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            // just returns the ShoppingCartItems for the given ShoppingCartId including the pies
            return ShoppingCartItems ??=
                       _bethanysPieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Pie)
                           .ToList();
        }

        public void ClearCart()
        {
            // will remove all the ShoppingCartItems for the given ShoppingCartId 
            var cartItems = _bethanysPieShopDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _bethanysPieShopDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _bethanysPieShopDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            // will do a calculation of the total for the ShoppingCart looping over all the ShoppingCartItems for the given ShoppingCartId
            // and multiplying for each the amount times the price and creating the sum of that
            var total = _bethanysPieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }
    }
}
