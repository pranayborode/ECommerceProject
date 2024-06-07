using ECommerceProject.Models;

namespace ECommerceProject.Services
{
	public interface ISubCategoryService
	{
		IEnumerable<SubCategory> GetSubCategories();
		SubCategory GetSubCategoryById(int id);
		int AddSubCategory(SubCategory subCategory);
		int EditSubCategory(SubCategory subCategory);
		int DeleteSubCategory(int id);

		IEnumerable<SubCategory> GetSubCategoriesByMainCategoryId(int mainCategoryId);
	}
}
