using ECommerceProject.Models;

namespace ECommerceProject.Services
{
	public interface IMainCategoryService
	{
		IEnumerable<MainCategory> GetMainCategories();
		MainCategory GetMainCategoryById(int id);
		int AddMainCategory(MainCategory mainCategory);
		int EditMainCategory(MainCategory mainCategory);
		int DeleteMainCategory(int id);
	}
}
