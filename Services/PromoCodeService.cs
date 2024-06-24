using ECommerceProject.Models;
using ECommerceProject.Repositories;
using System.Data;

namespace ECommerceProject.Services
{
	public class PromoCodeService : IPromoCodeService
	{
		private readonly IPromoCodeRepository repo;

        public PromoCodeService(IPromoCodeRepository repo)
        {
            this.repo = repo;
        }

        public void AddPromoCode(PromoCode promoCode)
		{
		   repo.AddPromoCode(promoCode);
		}

		

		public void DeletePromoCode(int promoCodeId)
		{
			repo.DeletePromoCode(promoCodeId);
		}

		public IEnumerable<PromoCode> GetActivePromoCodes()
		{
			return repo.GetActivePromoCodes();
		}

		public PromoCode GetPromoCodeByCodeName(string codeName)
		{
			return repo.GetPromoCodeByCodeName(codeName);
		}

		public PromoCode GetPromoCodeById(int promoCodeId)
		{
			return repo.GetPromoCodeById(promoCodeId);
		}

		public void UpdatePromoCode(PromoCode promoCode)
		{
			repo.UpdatePromoCode(promoCode);
		}

		public decimal ApplyPromoCode(string codeName, decimal orderAmount)
		{
			var promoCode = repo.GetPromoCodeByCodeName(codeName);

			if(promoCode == null || promoCode.StartDate > DateTime.Now || promoCode.EndDate< DateTime.Now)
			{
				throw new Exception("Invalid or expired promo code.");
			}

			if (orderAmount < promoCode.MinimumOrderAmount)
			{
				throw new Exception($"Order amount must be at least {promoCode.MinimumOrderAmount} to apply this promo code.");
			}

			decimal discountAmount = 0;

			if(promoCode.DiscountType == Helper.DiscountType.Percentage)
			{
				discountAmount = (promoCode.Discount / 100) * orderAmount;
				if(discountAmount > promoCode.MaxDiscountAmount)
				{
					discountAmount = promoCode.MaxDiscountAmount;
				}
			}
			else if(promoCode.DiscountType == Helper.DiscountType.Amount)
			{
				discountAmount = promoCode.Discount;

				if(discountAmount> promoCode.MaxDiscountAmount)
				{
					discountAmount = promoCode.MaxDiscountAmount;
				}
			}
			return discountAmount;

		}
	}
}
