using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
	public class QuickViewModel
	{
        internal object PaymetnStatus;

        public Product Product { get; set; }
		public IEnumerable<Product> SimilarProducts { get; set; }

	}
}
