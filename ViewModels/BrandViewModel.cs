using ECommerceProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.ViewModels
{
	public class BrandViewModel
	{
		public int BrandId { get; set; }
		public string Name { get; set; }

		public IFormFile? Image { get; set; }
		public string ImagePath { get; set; }
		public virtual ICollection<Product> Products { get; set; } = new List<Product>();

	}
}
