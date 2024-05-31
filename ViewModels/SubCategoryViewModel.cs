using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Subtitle { get; set; }

        public IFormFile Image { get; set; }

        public int MainCategoryId { get; set; }

        public List<MainCategory> MainCategories { get; set; }

        public string ImagePath { get; set; }
    }
}
