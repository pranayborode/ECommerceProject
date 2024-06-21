using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Razorpay.Api;
using ECommerceProject.Helper;
using Org.BouncyCastle.Asn1.X509;


namespace ECommerceProject.Controllers
{
	public class OrderController : Controller
	{
		[BindProperty]
		public PaymentViewModel _PaymentViewModel { get; set; }

		private readonly ICartService _cartService;
		private readonly IAddressService _addressService;
		private readonly IOrderService _orderService;
		private readonly IProductService _productService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ApplicationDbContext _context;
		public OrderController(
			ICartService cartService,
			IAddressService addressService,
			IOrderService orderService,
			UserManager<IdentityUser> userManager,
			IProductService productService,
			ApplicationDbContext context)
		{
			_cartService = cartService;
			_addressService = addressService;
			_orderService = orderService;
			_userManager = userManager;
			_productService = productService;
			_context = context;

		}

		//===============Admin Side===================
		//----OrderList, 

		[Authorize(Roles = "Admin")]
		public IActionResult OrdersList()
		{
			var order = _orderService.GetOrdersDesByDate();
			return View(order);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			var order = _orderService.GetOrderById(id);
			if (order == null)
			{
				return NotFound();
			}
			var viewModel = new OrderEditViewModel
			{
				Order = order,
				OrderItems = order.OrderItems.ToList(),
				PaymentStatuses = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToList(),
				OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList()
			};
			return View("UpdateOrder", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(OrderEditViewModel viewModel)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var _order = _orderService.GetOrderById(viewModel.Order.OrderId);
			try
			{
				var order = new Models.Order
				{
					OrderId = viewModel.Order.OrderId,
					UserId = viewModel.Order.UserId,
					AddressId = viewModel.Order.AddressId,
					TotalAmount = viewModel.Order.TotalAmount,
					PaymentMethod = viewModel.Order.PaymentMethod,
					ReceiptId = viewModel.Order.ReceiptId,
					RazorpayOrderId = viewModel.Order.RazorpayOrderId,
					RazorpayPaymentId = viewModel.Order.RazorpayPaymentId,
					RazorpaySignature = viewModel.Order.RazorpaySignature,
					OrderStatus = viewModel.Order.OrderStatus,
					PaymentStatus = viewModel.Order.PaymentStatus,
					OrderDate = viewModel.Order.OrderDate,
					UpdatedDate = DateTime.Now,
					//OrderItems = _order.OrderItems.ToList()

				};
				int result = _orderService.EditOrder(order);
				if (result >= 1)
				{
					return RedirectToAction(nameof(Edit));
				}
				else
				{
					var _viewModel = new OrderEditViewModel
					{
						Order = order,
						OrderItems = order.OrderItems.ToList(),
						PaymentStatuses = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToList(),
						OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList()
					};
					ViewBag.ErrorMsg = "Something went wrong...";
					return View("UpdateOrder", _viewModel);
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}	
		}


		public ActionResult OrderDetails(int id)
		{
			var order = _orderService.GetOrderById(id);

			if (order == null)
			{
				return NotFound();
			}
			var viewModel = new OrderEditViewModel
			{
				Order = order,
				OrderItems = order.OrderItems.ToList(),
				PaymentStatuses = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToList(),
				OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList()
			};
			return View("OrderDetails", viewModel);
			
		}
		public ActionResult Invoice(int id)
		{
			var order = _orderService.GetOrderById(id);

			if (order == null)
			{
				return NotFound();
			}
			var viewModel = new OrderEditViewModel
			{
				Order = order,
				OrderItems = order.OrderItems.ToList(),
				PaymentStatuses = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToList(),
				OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList()
			};
			return View("InvoiceDetails", viewModel);

		}


		public async Task<IActionResult> Delete(int id)
		{
			var model = _orderService.DeleteOrder(id);
			return View(model);
		}

		//================Client Side==================
		//------Checkout, SelectAddress , PlaceOrder , Payment , Success


		[Authorize(Roles = "Customer")]
		public IActionResult Checkout()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = _cartService.GetCartByUserId(userId);
			var addresses = _addressService.GetAddressesByUserId(userId);
			var viewModel = new CheckoutViewModel
			{
				CartItems = cart.CartItems,
				TotalAmount = cart.TotalAmount,
				Addresses = addresses

			};
			return View(viewModel);
		}

		[Authorize(Roles = "Customer")]
		[HttpPost]
		public IActionResult SelectAddress(int selectedAddressId)
		{
			var address = _addressService.GetAddressById(selectedAddressId);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = _cartService.GetCartByUserId(userId);
			var addresses = _addressService.GetAddressesByUserId(userId);
			var viewModel = new CheckoutViewModel
			{
				SelectedAddressId = selectedAddressId,
				SelectedAddress = address,
				CartItems = cart.CartItems,
				TotalAmount = cart.TotalAmount,
				Addresses = addresses
			};
			return View("Checkout", viewModel);
		}

		[Authorize(Roles = "Customer")]
		[HttpPost]
		public IActionResult PlaceOrder(CheckoutViewModel model)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = _userManager.FindByIdAsync(userId);
			var userEmail = user.Result.Email;
			var cart = _cartService.GetCartByUserId(userId);
			var selectedAddress = _addressService.GetAddressById(model.SelectedAddressId);
			var selectedAddressId = model.SelectedAddressId;
			if (selectedAddress == null)
			{
				// Handle case where the selected address does not exist
				ModelState.AddModelError("", "Selected address is invalid.");
				return View("Checkout", model);
			}
			var amount = (int)(cart.TotalAmount * 100);
			Random _random = new Random();
			int randomNumber = _random.Next(10000, 99999);
			string ReceiptId = $"REC-{DateTime.UtcNow:MMddHHmmss}-{randomNumber}";


			if (model.SelectedPaymentMethod == "Razorpay")
			{
				string key = "rzp_test_fJEgCGszLGlH7X";
				string secret = "EtvpucYs5NFNCGsw9zXxeD0q";



				Dictionary<string, object> input = new Dictionary<string, object>();
				input.Add("amount", amount);
				input.Add("currency", "INR");
				input.Add("receipt", ReceiptId);

				RazorpayClient client = new RazorpayClient(key, secret);
				Razorpay.Api.Order order = client.Order.Create(input);
				String orderId = order["id"].ToString();

				var paymentViewModel = new PaymentViewModel
				{
					Amount = amount,
					OrderId = orderId,
					CompanyName = "COZA STORE",
					CompanyLogo = "https://themewagon.github.io/cozastore/images/icons/logo-01.png",
					FullName = selectedAddress.FullName,
					Email = userEmail,
					Contact = selectedAddress.MobileNumber,
					Address = selectedAddress.City,
					AddressId = selectedAddressId,
					PaymentMethod = model.SelectedPaymentMethod,
					ReceiptId = ReceiptId
				};

				return View("Payment", paymentViewModel);
			}
			else if (model.SelectedPaymentMethod == "COD")
			{
				var order = new Models.Order
				{
					UserId = userId,
					AddressId = model.SelectedAddressId,
					TotalAmount = cart.TotalAmount,
					ReceiptId = ReceiptId,
					PaymentMethod = model.SelectedPaymentMethod,
					OrderStatus = OrderStatus.Confirmed,
					OrderDate = DateTime.Now,
					OrderItems = cart.CartItems.Select(item => new OrderItem
					{
						ProductId = item.ProductId,
						Quantity = item.Quantity,
						Price = item.Price,
						DiscountedPrice = item.DiscountedPrice,
						Size = item.Size
					}).ToList()
				};

				_orderService.AddOrder(order);

				// Update product stock
				foreach (var item in order.OrderItems)
				{
					var product = _productService.GetProductById(item.ProductId);
					if (product != null)
					{
						product.Stock -= item.Quantity;
						if (product.Stock <= 0)
						{
							product.IsAvailable = false;
						}
						_productService.EditProduct(product);
					}
				}
				_cartService.ClearCart(userId);

				return View("Success", order);
			}
			else
			{
				return View();
			}
		}

		[Authorize(Roles = "Customer")]
		public IActionResult Payment(string razorpay_payment_id, string razorpay_order_id,
			string razorpay_signature, int addressId, string paymentMethod)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var cart = _cartService.GetCartByUserId(userId);

			string key = "rzp_test_fJEgCGszLGlH7X";
			string secret = "EtvpucYs5NFNCGsw9zXxeD0q";
			try
			{
				RazorpayClient client = new RazorpayClient(key, secret);

				Dictionary<string, string> attributes = new Dictionary<string, string>();
				attributes.Add("razorpay_payment_id", razorpay_payment_id);
				attributes.Add("razorpay_order_id", Request.Form["razorpay_order_id"]);
				attributes.Add("razorpay_signature", Request.Form["razorpay_signature"]);

				Utils.verifyPaymentSignature(attributes);
				string receiptId = Request.Form["receipt"];
				var order = new Models.Order
				{
					UserId = userId,
					AddressId = addressId,
					TotalAmount = cart.TotalAmount,
					PaymentMethod = paymentMethod,
					ReceiptId = receiptId,
					OrderStatus = OrderStatus.Confirmed,
					PaymentStatus = PaymentStatus.Paid,
					OrderDate = DateTime.Now,
					RazorpayPaymentId = razorpay_payment_id,
					RazorpayOrderId = razorpay_order_id,
					RazorpaySignature = razorpay_signature,
					OrderItems = cart.CartItems.Select(item => new OrderItem
					{
						ProductId = item.ProductId,
						Quantity = item.Quantity,
						Price = item.Price,
						DiscountedPrice = item.DiscountedPrice,
						Size = item.Size
					}).ToList()
				};

				_orderService.AddOrder(order);

				// Update product stock
				foreach (var item in order.OrderItems)
				{
					var product = _productService.GetProductById(item.ProductId);
					if (product != null)
					{
						product.Stock -= item.Quantity;
						if (product.Stock <= 0)
						{
							product.IsAvailable = false;
						}
						_productService.EditProduct(product);
					}
				}

				_cartService.ClearCart(userId);

				return RedirectToAction("Success", new { id = order.OrderId });
			}
			catch (Exception ex)
			{
				// Payment verification failed
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			};

		}


		// Success action to display order success message
		[Authorize(Roles = "Customer")]
		public IActionResult Success(int id)
		{
			var order = _orderService.GetOrderById(id);
			return View(order);
		}

		// Error action to display payment error message
		public IActionResult Error()
		{
			return View();
		}

	}
}
