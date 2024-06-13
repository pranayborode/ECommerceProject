using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
	public class MainCategoryService : IMainCategoryService
	{
		private readonly IMainCategoryRepository repo;

        public MainCategoryService(IMainCategoryRepository repo)
        {
			this.repo = repo;
        }

        public int AddMainCategory(MainCategory mainCategory)
		{
			return repo.AddMainCategory(mainCategory);
		}

		public int DeleteMainCategory(int id)
		{
			return repo.DeleteMainCategory(id);
		}

		public int EditMainCategory(MainCategory mainCategory)
		{
			return repo.EditMainCategory(mainCategory);
		}

		public IEnumerable<MainCategory> GetMainCategories()
		{
			return repo.GetMainCategories();
		}
		public MainCategory GetMainCategoryById(int id)
		{
			return repo.GetMainCategoryById(id);
		}
	}
}
