using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public interface IPromoCodeRepository
	{
		PromoCode GetPromoCodeById(int promoCodeId);
		PromoCode GetPromoCodeByCodeName(string codeName);
		IEnumerable<PromoCode> GetActivePromoCodes();
		void AddPromoCode(PromoCode promoCode);
		void UpdatePromoCode(PromoCode promoCode);
		void DeletePromoCode(int promoCodeId);
	}
}
