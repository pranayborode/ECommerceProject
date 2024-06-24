using ECommerceProject.Data;
using ECommerceProject.Helper;
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
            var existingOrder = _context.Orders.Find(order.OrderId);

            if (existingOrder == null)
            {
                return 0; // Return 0 or handle accordingly if order not found
            }

            // Update only the properties that are modified
            _context.Entry(existingOrder).CurrentValues.SetValues(order);

            // Ensure related entities are not modified unintentionally
            _context.Entry(existingOrder).Collection(o => o.OrderItems).IsModified = false; // Assuming OrderItems should not be updated here

            return _context.SaveChanges();
            /*var _order = _context.Orders.Find(order.OrderId);
            if (order != null)
            {
                _context.Orders.Update(order);
                return _context.SaveChanges();
            }
            else
            {
                return 0;
            }*/

        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
        .Include(o => o.Address)
        .FirstOrDefault(o => o.OrderId == id);

            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }
        public IEnumerable<Order> GetOrdersDesByDate()
        {

            return _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                .ToList();
        }

        public int GetPendingOrderCount()
        {
            var pendingOrders = _context.Orders
                                .Where(o => o.OrderStatus == OrderStatus.Confirmed)
                                .ToList();

            return pendingOrders.Count;
        }

        public IEnumerable<Order> GetOrdersByUserId(string userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

    }
}
