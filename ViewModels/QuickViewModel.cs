using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
	public class QuickViewModel
	{
		public Product Product { get; set; }
		public IEnumerable<Product> SimilarProducts { get; set; }
	}
}
