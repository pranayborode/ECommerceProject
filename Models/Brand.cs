using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models
{
	public class Brand
	{
		[Key]
		public int BrandId { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		public string? Image { get; set; }

		// Navigation property for Products
		public virtual ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
