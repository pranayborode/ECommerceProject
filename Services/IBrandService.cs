using ECommerceProject.Models;

namespace ECommerceProject.Services
{
	public interface IBrandService
	{
		IEnumerable<Brand> GetBrands();
		Brand GetBrandById(int id);
		int AddBrand(Brand brand);
		int EditBrand(Brand brand);
		int DeleteBrand(int id);
	}
}
