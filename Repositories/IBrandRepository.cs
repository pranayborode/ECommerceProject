using ECommerceProject.Models;
using System.Drawing.Drawing2D;

namespace ECommerceProject.Repositories
{
	public interface IBrandRepository
	{
		IEnumerable<Brand> GetBrands();
		Brand GetBrandById(int id);
		int AddBrand(Brand brand);
		int EditBrand(Brand brand);
		int DeleteBrand(int id);
	}
}
