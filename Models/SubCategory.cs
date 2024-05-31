using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
	public class SubCategory
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Subtitle { get; set; }

		public string? Image { get; set; }

		// Foreign key property
		public int MainCategoryId { get; set; }

		// Navigation property for MainCategory
		[ForeignKey("MainCategoryId")]
		public virtual MainCategory MainCategory { get; set; }

		// Navigation property for Products
		public virtual ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
