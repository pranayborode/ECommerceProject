using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
	public class BrandService : IBrandService
	{
		private readonly IBrandRepository repo;
		public BrandService(IBrandRepository repo)
		{
			this.repo = repo;
		}

		public int AddBrand(Brand brand)
		{
			return repo.AddBrand(brand);
		}

		public int DeleteBrand(int id)
		{
			return repo.DeleteBrand(id);
		}

		public int EditBrand(Brand brand)
		{
			return repo.EditBrand(brand);
		}

		public Brand GetBrandById(int id)
		{
			return repo.GetBrandById(id);
		}

		public IEnumerable<Brand> GetBrands()
		{
			return repo.GetBrands();
		}
	}
}
