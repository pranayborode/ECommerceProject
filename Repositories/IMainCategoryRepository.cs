using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public interface IMainCategoryRepository
	{
		IEnumerable<MainCategory> GetMainCategories();
		MainCategory GetMainCategoryById(int id);
		int AddMainCategory (MainCategory mainCategory);
		int EditMainCategory(MainCategory mainCategory);
		int DeleteMainCategory(int id);
	}
}
