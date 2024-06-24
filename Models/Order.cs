using ECommerceProject.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerceProject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
		public string ReceiptId { get; set; }

		public string? RazorpayPaymentId { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string? RazorpaySignature { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

		public DateTime? DeliveryDate { get; set; }
		public string? DeliveryMessage { get; set; }
	}
}
