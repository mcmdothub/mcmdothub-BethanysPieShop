namespace mcmdothub_BethanysPieShop.Models
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}