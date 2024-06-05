using ECommerceProject.Data;
using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext db;
		public ProductRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddProduct(Product product)
		{
			int result = 0;
			db.Products.Add(product);
			result = db.SaveChanges();
			return result;
		}

		public int DeleteProduct(int id)
		{
			var product = db.Products.Find(id);
			if(product != null)
			{
				db.Products.Remove(product);
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public int EditProduct(Product product)
		{
			var pro = db.Products.Find(product.ProductId);
			if(pro != null)
			{
				pro.Name = product.Name;
				pro.Description = product.Description;
				pro.Price = product.Price;
				pro.Stock = product.Stock;
				pro.IsAvailable = product.IsAvailable;
				pro.SKU = product.SKU;
				pro.Image = product.Image;
				pro.OfferPercentage = product.OfferPercentage;
				pro.MainCategory = product.MainCategory;
				pro.SubCategory = product.SubCategory;
				pro.Brand = product.Brand;
				int result = db.SaveChanges();
				return result;

			}
			else
			{
				return 0;
			}
		}

		public Product GetProductById(int id)
		{
			var product = db.Products.Find(id);
			if(product != null)
			{
				return product;
			}
			else
			{
				return null;
			}
		}

		public IEnumerable<Product> GetProducts()
		{
			return db.Products.ToList();
		}
	}
}
