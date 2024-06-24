using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public interface IProductRepository
	{
		IEnumerable<Product> GetProducts();

		IEnumerable<Product> GetActiveProducts();

		Product GetProductById(int id);
		int AddProduct(Product product);
		int EditProduct(Product product);
		int DeleteProduct(int id);
        int GetSoldOutProductsCount();
		public IEnumerable<Product> GetSimilarProducts(int productId);

		

	}
}
