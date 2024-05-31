using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
	public class SubCategoryService : ISubCategoryService
	{
		private readonly ISubCategoryRepository repo;

        public SubCategoryService(ISubCategoryRepository repo)
        {
            this.repo = repo;
        }

        public int AddSubCategory(SubCategory subCategory)
		{
			return repo.AddSubCategory(subCategory);
		}

		public int DeleteSubCategory(int id)
		{
			return repo.DeleteSubCategory(id);
		}

		public int EditSubCategory(SubCategory subCategory)
		{
			return repo.EditSubCategory(subCategory);
		}

		public IEnumerable<SubCategory> GetSubCategories()
		{
			return repo.GetSubCategories();
		}

		public SubCategory GetSubCategoryById(int id)
		{
			return repo.GetSubCategoryById(id);
		}
	}
}
