using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository repo;

        public OrderService(IOrderRepository repo)
        {
            this.repo = repo;
        }
        public int AddOrder(Order order)
        {
            return repo.AddOrder(order);    
        }

        public int DeleteOrder(int id)
        {
            return repo.DeleteOrder(id);
        }

        public int EditOrder(Order order)
        {
            return repo.EditOrder(order);
        }

        public Order GetOrderById(int id)
        {
           return repo.GetOrderById(id);
        }

        public IEnumerable<Order> GetOrders()
        {
           return repo.GetOrders();
        }

		public int GetPendingOrderCount()
		{
            return repo.GetPendingOrderCount();
		}
	}
}
