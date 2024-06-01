using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        public string? Image { get; set; }

        // Offer percentage
        [Range(0, 100)]
        public int OfferPercentage { get; set; } 

        [Required]
        public string Gender { get; set; } 
         
        // Foreign key properties
        public int MainCategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int BrandId { get; set; }

        // Navigation properties
        [ForeignKey("MainCategoryId")]
        public virtual MainCategory MainCategory { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
