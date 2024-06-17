using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

    }
}
