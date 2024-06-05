using ECommerceProject.Data;
using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	
	public class BrandRepository : IBrandRepository
	{
		public readonly ApplicationDbContext db;

        public BrandRepository(ApplicationDbContext db)
        {
            this.db=db;
        }
        public int AddBrand(Brand brand)
		{
			int result = 0;
			db.Brands.Add(brand);
			result = db.SaveChanges();
			return result;
		}

		public int DeleteBrand(int id)
		{
			var brand = db.Brands.Find(id);
			if(brand != null)
			{
				db.Brands.Remove(brand);
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public int EditBrand(Brand brand)
		{
			var _brand = db.Brands.Find(brand.BrandId);
			if(_brand != null)
			{
				_brand.BrandId = brand.BrandId;
				_brand.Name = brand.Name;
				_brand.Image = brand.Image;
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public Brand GetBrandById(int id)
		{
			var brand = db.Brands.Find(id);
			if (brand != null)
			{
				return brand;
			}
			else
			{
				return 0;
			}
		}

		public IEnumerable<Brand> GetBrands()
		{
			return db.Brands.ToList();
		}
	}
}
