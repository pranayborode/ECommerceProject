using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public interface ISubCategoryRepository
	{
		IEnumerable<SubCategory> GetSubCategories();
		SubCategory GetSubCategoryById(int id);
		int AddSubCategory(SubCategory subCategory);
		int EditSubCategory(SubCategory subCategory);
		int DeleteSubCategory(int id);
	}
}
