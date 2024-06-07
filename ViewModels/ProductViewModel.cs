using ECommerceProject.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.ViewModels
{
	public class ProductViewModel
	{
		public int ProductId { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }


		public string Description { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }
       

        [Required]
		public int Stock { get; set; }

		[Required]
		public bool IsAvailable { get; set; }

		

		public IFormFile Image { get; set; }
		public string ImagePath { get; set; }

		// Offer percentage
		[Range(0, 100)]
		public int OfferPercentage { get; set; }

		[Display(Name = "MainCategory")]
		public int MainCategoryId { get; set; }

		[Display(Name = "SubCategory")]
		public int SubCategoryId { get; set; }

		[Display(Name = "Brand")]
		[Required]
		public int BrandId { get; set; }

		public List<MainCategory> MainCategories { get; set; }

		public List<SubCategory> SubCategories { get; set; }

		public List<Brand> Brands { get; set; }

		public virtual ICollection<ProductImage> Images { get; set; }
	}
}
