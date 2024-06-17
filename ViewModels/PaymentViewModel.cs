using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ECommerceProject.ViewModels
{
    public class PaymentViewModel
    {
        public int Amount { get; set; } // Amount in paise
        public string OrderId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public int AddressId { get; set; }

        public string PaymentMethod { get; set; }

    }
}
