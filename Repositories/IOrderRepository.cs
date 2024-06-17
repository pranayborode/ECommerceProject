using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrderById(int id);
        int AddOrder(Order order);
        int EditOrder(Order order);
        int DeleteOrder(int id);
        int GetPendingOrderCount();


	}
}
