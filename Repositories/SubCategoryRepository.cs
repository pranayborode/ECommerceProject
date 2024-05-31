using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Repositories
{
	public class SubCategoryRepository : ISubCategoryRepository
	{
		public readonly ApplicationDbContext db;

		public SubCategoryRepository(ApplicationDbContext db)
		{
			this.db = db;
		}
		public int AddSubCategory(SubCategory subCategory)
		{
			int result = 0;
			db.SubCategories.Add(subCategory);
			result = db.SaveChanges();
			return result;
		}

		public int DeleteSubCategory(int id)
		{
			var subCategory = db.SubCategories.Find(id);
			if(subCategory != null)
			{
				db.SubCategories.Remove(subCategory);
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public int EditSubCategory(SubCategory subCategory)
		{
			var sCat = db.SubCategories.Find(subCategory.Id);
			if(subCategory != null)
			{
				sCat.Name = subCategory.Name;
				sCat.Subtitle = subCategory.Subtitle;
				sCat.Image = subCategory.Image;
				sCat.MainCategoryId = subCategory.MainCategoryId;
				int result = db.SaveChanges();
				return result;
			}
			else
			{
				return 0;
			}
		}

		public IEnumerable<SubCategory> GetSubCategories()
		{
			return db.SubCategories.Include(sc =>sc.MainCategory).ToList();
		}

		public SubCategory GetSubCategoryById(int id)
		{
			var subCategory = db.SubCategories.Include(sc => sc.MainCategory).FirstOrDefault(sc => sc.Id == id);
			if (subCategory != null)
			{
				return subCategory;
			}
			else
			{
				return null;
			}
		}
	}
}
