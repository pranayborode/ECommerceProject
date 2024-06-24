using ECommerceProject.Helper;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.ViewModels
{
	public class PromoCodeViewModel
	{
        public PromoCode PromoCode { get; set; }
      
        public List<DiscountType> DiscountTypes { get; set; }
      
		public List<RepeatUsage> RepeatUsages { get; set; }

		public List<PromoCodeStatus> PromoCodeStatuses { get; set; }

	}
   
}
