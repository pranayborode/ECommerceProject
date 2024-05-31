using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
	public class MainCategoryViewModel
	{
		public int Id { get; set; }

		
		public string Name { get; set; }

		public string Subtitle { get; set; }

		
		public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

       
    }
}
