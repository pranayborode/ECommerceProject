using ECommerceProject.Data;
using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public class MainCategoryRepository : IMainCategoryRepository
	{
		private readonly ApplicationDbContext db;

        public MainCategoryRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddMainCategory(MainCategory mainCategory)
		{
			int result = 0;
			db.MainCategories.Add(mainCategory);
			result = db.SaveChanges();
			return result;
		}

		public int DeleteMainCategory(int id)
		{
			var mainCategory = db.MainCategories.Find(id);
			if(mainCategory != null)
			{
				db.MainCategories.Remove(mainCategory);
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public int EditMainCategory(MainCategory mainCategory)
		{
			var mCat = db.MainCategories.Find(mainCategory.Id);
			if(mCat != null)
			{
				mCat.Name = mainCategory.Name;
				mCat.Subtitle = mainCategory.Subtitle;
				mCat.Image = mainCategory.Image;
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public IEnumerable<MainCategory> GetMainCategories()
		{
			return db.MainCategories.ToList();
		}

		public MainCategory GetMainCategoryById(int id)
		{
			var mainCategory = db.MainCategories.Find(id);
			if(mainCategory != null)
			{
				return mainCategory;
			}
			else
			{
				return null;
			}
		}
	}
}
