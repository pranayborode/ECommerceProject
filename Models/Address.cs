using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string Pincode { get; set; }

        [Required]
        [StringLength(200)]
        public string Apartment { get; set; }

        [Required]
        [StringLength(200)]
        public string Street { get; set; }

        [StringLength(200)]
        public string Landmark { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

		[NotMapped]
		public string FullAddress => $"{FullName}, {Apartment}, {Street}, {Landmark}, {City}, {State}, {Pincode}, {Country}";

		[NotMapped]
		public string FullAddressWTOname => $"{Apartment}, {Street}, {Landmark}, {City}, {State}, {Pincode}, {Country}";



	}
}
