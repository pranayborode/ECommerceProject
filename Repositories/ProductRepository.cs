using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;
		public ProductRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public int AddProduct(Product product)
		{
			int result = 0;
			_context.Products.Add(product);
			result = _context.SaveChanges();
			return result;
		}

		public int DeleteProduct(int id)
		{
			var product = _context.Products.Find(id);
			if(product != null)
			{
				_context.Products.Remove(product);
				int result = _context.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public int EditProduct(Product product)
		{
			var pro = _context.Products.Find(product.ProductId);
			if(pro != null)
			{
				pro.Name = product.Name;
				pro.Description = product.Description;
				pro.Price = product.Price;
				pro.Stock = product.Stock;
				pro.IsAvailable = product.IsAvailable;
				pro.Image = product.Image;
				pro.OfferPercentage = product.OfferPercentage;
				pro.MainCategory = product.MainCategory;
				pro.SubCategory = product.SubCategory;
				pro.Brand = product.Brand;
				int result = _context.SaveChanges();
				return result;

			}
			else
			{
				return 0;
			}
		}

		public Product GetProductById(int id)
		{
			var product = _context.Products
				.Include(p => p.MainCategory)
                .Include(p => p.SubCategory)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductId == id);
            if (product != null)
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
			return _context.Products
				.Include(p=>p.MainCategory)
				.Include(p=>p.SubCategory)
				.Include(p=>p.Brand)
				.ToList();
		}

        // To GET Similar Products by SubCategory in QuickView 
        public IEnumerable<Product> GetSimilarProducts(int productId)
        {
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                var subcategoryProducts = _context.Products
                    .Where(p => p.SubCategoryId == product.SubCategoryId && p.ProductId != productId)
                    .ToList();

                if (subcategoryProducts.Count < 4)
                {
					var mainCategoryProducts = _context.Products
                        .Where(p => p.MainCategoryId == product.MainCategoryId && p.ProductId != productId)
                        .Take(4 - subcategoryProducts.Count) 
                        .ToList();

                   
                    return subcategoryProducts.Concat(mainCategoryProducts);
                }
                else
                {
                    
                    return subcategoryProducts;
                }
            }

            return new List<Product>(); // Return an empty list if the product is not found
        }



        // To Display SoldOut Products Count in Dashboard 
        public int GetSoldOutProductsCount()
        {
            return _context.Products.Count(p => !p.IsAvailable);
        }
    }
}

/*
public IEnumerable<Product> GetSimilarProducts(int productId)
{
    var product = _context.Products.Find(productId);
    if (product != null)
    {
        return _context.Products
        .Where(p => p.SubCategoryId == product.SubCategoryId && p.ProductId != productId)
        .ToList();
    }
    return new List<Product>();
}*/