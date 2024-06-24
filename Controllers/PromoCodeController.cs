using ECommerceProject.Helper;
using ECommerceProject.Models;
using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService _promoCodeService;
        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        // GET: PromoCodeController
        [HttpGet]
        public ActionResult Index()
        {
            var promoCodes = _promoCodeService.GetActivePromoCodes();
            return View(promoCodes);
        }


        // GET: PromoCodeController/Create
        public ActionResult Create()
        {
            var model = new PromoCode
            {
                DiscountTypes = Enum.GetValues(typeof(DiscountType)).Cast<DiscountType>().ToList(),
                RepeatUsages = Enum.GetValues(typeof(RepeatUsage)).Cast<RepeatUsage>().ToList(),
                PromoCodeStatuses = Enum.GetValues(typeof(PromoCodeStatus)).Cast<PromoCodeStatus>().ToList()
            };
            return View(model);
        }

        // POST: PromoCodeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PromoCode promoCode)
        {
            try
            {
                _promoCodeService.AddPromoCode(promoCode);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        // GET: PromoCodeController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var promoCode = _promoCodeService.GetPromoCodeById(id);

            if (promoCode == null)
            {
                return NotFound();
            }

            var viewModel = new PromoCodeViewModel
            {
                PromoCode = promoCode,
                DiscountTypes = Enum.GetValues(typeof(DiscountType)).Cast<DiscountType>().ToList(),
                RepeatUsages = Enum.GetValues(typeof(RepeatUsage)).Cast<RepeatUsage>().ToList(),
                PromoCodeStatuses = Enum.GetValues(typeof(PromoCodeStatus)).Cast<PromoCodeStatus>().ToList()
            };

            return View(viewModel);
        }


        // POST: PromoCodeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PromoCodeViewModel viewModel)
        {
            try
            {
                var promoCode = new PromoCode
                {
                    PromoCodeId = viewModel.PromoCode.PromoCodeId,
                    CodeName = viewModel.PromoCode.CodeName,
                    Message = viewModel.PromoCode.Message,
                    StartDate = viewModel.PromoCode.StartDate,
                    EndDate = viewModel.PromoCode.EndDate,
                    NoOfUsers = viewModel.PromoCode.NoOfUsers,
                    MinimumOrderAmount = viewModel.PromoCode.MinimumOrderAmount,
                    Discount = viewModel.PromoCode.Discount,
                    DiscountType = viewModel.PromoCode.DiscountType,
                    MaxDiscountAmount = viewModel.PromoCode.MaxDiscountAmount,
                    RepeatUsage = viewModel.PromoCode.RepeatUsage,
                    PromoCodeStatus = viewModel.PromoCode.PromoCodeStatus
                };

                _promoCodeService.UpdatePromoCode(promoCode);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        // GET: PromoCodeController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var promoCode = _promoCodeService.GetPromoCodeById(id);
            if (promoCode == null)
            {
                return NotFound();
            }
            return View(promoCode);
        }


        // POST: PromoCodeController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _promoCodeService.DeletePromoCode(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


		[HttpGet]
		public IActionResult ApplyPromoCode(string codeName, decimal orderAmount)
		{
			var response = new { success = false, discountAmount = 0.0m, message = "Invalid promo code." };

			try
			{
				var discount = _promoCodeService.ApplyPromoCode(codeName, orderAmount);
				if (discount > 0)
				{
					response = new { success = true, discountAmount = discount, message = "Promo code applied successfully." };
				}
			}
			catch (Exception ex)
			{
				response = new { success = false, discountAmount = 0.0m, message = ex.Message };
			}

			return Json(response);
		}

	}
}
