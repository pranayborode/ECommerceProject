using ECommerceProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceProject.Controllers
{
	[Authorize(Roles = "Customer")]
	public class CartController : Controller
	{

		private readonly ICartService _cartService;
		private readonly IProductService _productService;
        public CartController(
			ICartService _cartService, 
			IProductService _productService)
        {
            this._cartService = _cartService;
			this._productService = _productService;
        }

        public ActionResult Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = _cartService.GetCartByUserId(userId);

			ViewBag.TotalQuantity = cart.TotalQuantity;

			return View(cart);
		}

	
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult AddToCart(int productId, int quantity, string size = null)
		{
		
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var product = _productService.GetProductById(productId);

                var cartItem = new CartItem
				{
					UserId = userId,
					ProductId = productId,
					Quantity = quantity,
					Size = size,
					Price = product.Price,
					DiscountedPrice = product.Price - (product.Price * product.OfferPercentage / 100),
					OfferPercentage = product.OfferPercentage,
					CreatedDate = DateTime.Now
				};

				int result = _cartService.AddToCart(cartItem);
                return RedirectToAction("Index");

              /*  if (result >= 1)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.ErrorMsg = "Something went wrong...";
					return View();
				}*/
				
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RemoveFromCart(int cartItemId)
		{
			try
			{
				_cartService.RemoveFromCart(cartItemId);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ClearCart()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				_cartService.ClearCart(userId);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

		[HttpPost]
		public IActionResult UpdateQuantity(int cartItemId, int quantity)
		{
			try
			{
				_cartService.UpdateCartItemQuantity(cartItemId, quantity);
				return Ok("Quantity updated successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest("Error updating quantity: " + ex.Message);
			}
		}


		// Get: To show Cart Items Count in Navbar - _Logout.cshtml
		[Authorize(Roles = "Customer")]
		public JsonResult GetCartItemCount()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = _cartService.GetCartByUserId(userId);
			var itemCount = cart?.TotalQuantity ?? 0;
			return Json(itemCount);
		}
	}
}
