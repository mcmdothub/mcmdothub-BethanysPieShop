namespace mcmdothub_BethanysPieShop.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;
        private readonly IShoppingCart _shoppingCart;

        // need the DbContext + shoppingCart
        // Bring them in using dependency injection
        public OrderRepository(BethanysPieShopDbContext bethanysPieShopDbContext, IShoppingCart shoppingCart)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
            _shoppingCart = shoppingCart;
        }

        // We receive an order (Order order) that i need to save to the database
        public void CreateOrder(Order order)
        {
            // we update the OrderPlaced to DateTime.Now
            order.OrderPlaced = DateTime.Now;

            // we look inside the shoppingCart for the ShoppingCartItems
            List<ShoppingCartItem>? shoppingCartItems = _shoppingCart.ShoppingCartItems;

            // ask the shoppingCart for the Total
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            // we create the OrderDetails
            order.OrderDetails = new List<OrderDetail>();

            // and do it by looping over the shoppingCartItems
            foreach (ShoppingCartItem? shoppingCartItem in shoppingCartItems)
            {
                // create a new orderDetail for each shoppingCartItem and map the Amount,PieId and Price into the OrderDetail
                var orderDetail = new OrderDetail
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    Price = shoppingCartItem.Pie.Price
                };

                // we add that to the OrderDetails list of the order
                order.OrderDetails.Add(orderDetail);
            }

            // add the order to the DbContext
            _bethanysPieShopDbContext.Orders.Add(order);

            // save changes
            _bethanysPieShopDbContext.SaveChanges();
        }
    }
}