using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models
{
	public class MainCategory
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Subtitle { get; set; }

		public string? Image { get; set; }

		// Navigation property for SubCategoriesS
		public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

		// Navigation property for Products
		public virtual ICollection<Product> Products { get; set; } = new List<Product>();
	}
}
