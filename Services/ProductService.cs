using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository repo;

        public ProductService(IProductRepository repo)
        {
            this.repo = repo;
        }
        public int AddProduct(Product product)
		{
			return repo.AddProduct(product);
		}

		public int DeleteProduct(int id)
		{
			return repo.DeleteProduct(id);
		}

		public int EditProduct(Product product)
		{
			return repo.EditProduct(product);
		}

		public IEnumerable<Product> GetActiveProducts()
		{
			return repo.GetActiveProducts();
		}

		public Product GetProductById(int id)
		{
			return repo.GetProductById(id);
		}

		public IEnumerable<Product> GetProducts()
		{
			return repo.GetProducts();
		}

		public IEnumerable<Product> GetSimilarProducts(int productId)
		{
			return repo.GetSimilarProducts(productId);
		}

		public int GetSoldOutProductsCount()
        {
            return repo.GetSoldOutProductsCount();
        }
    }
}
