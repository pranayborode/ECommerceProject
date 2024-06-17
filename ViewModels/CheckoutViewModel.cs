using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class CheckoutViewModel
    {
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        //public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
        public int SelectedAddressId { get; set; }
        public Address SelectedAddress { get; set; } 
        public string SelectedPaymentMethod { get; set; }
    }
}
