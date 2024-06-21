using ECommerceProject.Helper;
using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class OrderEditViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<PaymentStatus> PaymentStatuses { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }

		
	}
}
