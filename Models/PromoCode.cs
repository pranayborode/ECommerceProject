using ECommerceProject.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
	public class PromoCode
	{
		[Key]
		public int PromoCodeId { get; set; }

		[Display(Name = "Code")]
		[Required(ErrorMessage = "Promo code is required.")]
		[StringLength(20, ErrorMessage = "Promo code must be between {2} and {1} characters long.", MinimumLength = 3)]
		public string CodeName { get; set; }

		[StringLength(100, ErrorMessage = "Message cannot exceed {1} characters.")]
		public string Message { get; set; }

		[Display(Name = "Start Date")]
		[DataType(DataType.Date)]
		public DateTime? StartDate { get; set; }

		[Display(Name = "End Date")]
		[DataType(DataType.Date)]
		[PromoCodeEndDate(ErrorMessage = "End Date must be greater than Start Date.")]
		public DateTime? EndDate { get; set; }

		[Display(Name = "No. of Users")]
		[Range(1, int.MaxValue, ErrorMessage = "Number of users must be greater than 0.")]
		public int? NoOfUsers { get; set; }


		[Display(Name = "Minimum Order Amount")]
		[Range(0, double.MaxValue, ErrorMessage = "Minimum order amount must be greater than or equal to 0.")]
		public int MinimumOrderAmount { get; set; }

		[Display(Name = "Discount")]
		[Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0.")]
		public int Discount { get; set; }

		[Display(Name = "Discount Type")]
		public DiscountType DiscountType { get; set; }

		[Display(Name = "Max Discount Amount")]
		[Range(0, double.MaxValue, ErrorMessage = "Max discount amount must be greater than or equal to 0.")]
		public int MaxDiscountAmount { get; set; }

		[Display(Name = "Repeat Usage")]
		public RepeatUsage RepeatUsage {  get; set; }

		[Display(Name = "Status")]
		public PromoCodeStatus PromoCodeStatus { get; set; }

        [NotMapped]
        public List<DiscountType> DiscountTypes { get; set; }
        [NotMapped]
        public List<RepeatUsage> RepeatUsages { get; set; }
        [NotMapped]
        public List<PromoCodeStatus> PromoCodeStatuses { get; set; }

    }

	public class PromoCodeEndDateAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var promoCode = (PromoCode)validationContext.ObjectInstance;

			if (promoCode.StartDate.HasValue && promoCode.EndDate.HasValue)
			{
				if (promoCode.EndDate < promoCode.StartDate)
				{
					return new ValidationResult(ErrorMessage);
				}
			}

			return ValidationResult.Success;
		}
	}
}
