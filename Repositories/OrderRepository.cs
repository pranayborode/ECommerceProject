using ECommerceProject.Data;
using ECommerceProject.Enum;
using ECommerceProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public int AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return _context.SaveChanges();
        }

        public int DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int EditOrder(Order order)
        {
            var _order = _context.Orders.Find(order.OrderId);
            if (order != null)
            {
                _context.Orders.Update(order);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)  // Include product details for each order item
        .Include(o => o.Address)  // Include address details
        .FirstOrDefault(o => o.OrderId == id);

            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

		public int GetPendingOrderCount()
		{
			var pendingOrders = _context.Orders
								.Where(o => o.OrderStatus == OrderStatus.Confirmed)
								.ToList(); // Convert to list to force execution and inspect in debugger

			// Log or debug pendingOrders to check if it retrieves the correct orders

			return pendingOrders.Count;
		}
	}
}
