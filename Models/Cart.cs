using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
	public class Cart
	{
		[Key]
		public int CartId { get; set; }
		[Required]
		public string UserId { get; set; }
		public ICollection<CartItem> CartItems { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalAmount { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		public DateTime? UpdatedDate { get; set; }

		[NotMapped]
		public int TotalQuantity => CartItems?.Sum(item => item.Quantity) ?? 0;
	}
}
