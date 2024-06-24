using ECommerceProject.Models;

namespace ECommerceProject.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetOrdersDesByDate();
        IEnumerable<Order> GetOrdersByUserId(string userId);


        Order GetOrderById(int id);
        int AddOrder(Order order);
        int EditOrder(Order order);
        int DeleteOrder(int id);

	    int GetPendingOrderCount();

	}
}
