using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
	public class ProductImage
	{
		[Key]
		public int ProductImageId { get; set; }

		[NotMapped] // Not mapped to the database
		public byte[] ImageData { get; set; } // Byte array to store image data

		[Required]
		public string ImageUrl { get; set; } // URL for reference (optional)

		// Foreign key property
		public int ProductId { get; set; }

		// Navigation property
		public virtual Product Product { get; set; }
	}
}
