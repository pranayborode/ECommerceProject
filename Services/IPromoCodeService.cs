using ECommerceProject.Models;

namespace ECommerceProject.Services
{
	public interface IPromoCodeService
	{
		PromoCode GetPromoCodeById(int promoCodeId);
		PromoCode GetPromoCodeByCodeName(string codeName);
		IEnumerable<PromoCode> GetActivePromoCodes();
		void AddPromoCode(PromoCode promoCode);
		void UpdatePromoCode(PromoCode promoCode);
		void DeletePromoCode(int promoCodeId);
		decimal ApplyPromoCode(string codeName, decimal orderAmount);
	}
}
