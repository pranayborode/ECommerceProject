using ECommerceProject.Data;
using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public class PromoCodeRepository : IPromoCodeRepository
	{

		private readonly ApplicationDbContext _context;

        public PromoCodeRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

		public PromoCode GetPromoCodeById(int promoCodeId)
		{
			return _context.PromoCodes.Find(promoCodeId);
		}

		public PromoCode GetPromoCodeByCodeName(string codeName)
		{
			return _context.PromoCodes
				.FirstOrDefault(pc=>pc.CodeName == codeName && pc.PromoCodeStatus == Helper.PromoCodeStatus.Active);
		}

		public IEnumerable<PromoCode> GetActivePromoCodes()
		{
			return _context.PromoCodes
				.Where(pc => pc.PromoCodeStatus == Helper.PromoCodeStatus.Active).ToList();
		}
		public void AddPromoCode(PromoCode promoCode)
		{
			_context.PromoCodes.Add(promoCode);
			_context.SaveChanges();
		}

		public void UpdatePromoCode(PromoCode promoCode)
		{
			_context.PromoCodes.Update(promoCode);
			_context.SaveChanges();
		}
		public void DeletePromoCode(int promoCodeId)
		{
			var promoCode = _context.PromoCodes.Find(promoCodeId);
			if(promoCode != null)
			{
				_context.PromoCodes.Remove(promoCode);
				_context.SaveChanges();
			}
		}

		

		
		

	
	}
}
