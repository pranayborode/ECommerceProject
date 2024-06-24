using ECommerceProject.Data;
using ECommerceProject.Helper;
using ECommerceProject.Models;
using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommerceProject.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IProductService _productService;
		private readonly IOrderService _orderService;
		public HomeController(
			ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
			ApplicationDbContext context,
            IProductService productService,
			IOrderService orderService)
		{
			_logger = logger;
			_userManager = userManager;
			_context = context;
			_productService = productService;
			_orderService = orderService;
		}


        [AllowAnonymous]
        public IActionResult Index()
		{
	
            IEnumerable<Product> products = _productService.GetActiveProducts();
           
            if (products == null)
            {
                products = new List<Product>();
            }

            return View(products);
		}
		[HttpGet]
        public ActionResult UserPage()
        {
            var model = _productService.GetProducts();
            return PartialView("_UserPage", model);
        }

        public ActionResult QuickView(int id)
        {
            var product = _productService.GetProductById(id);
			var similarProducts = _productService.GetSimilarProducts(id);

			var viewModel = new QuickViewModel
			{
				Product = product,
				SimilarProducts = similarProducts
				
            };
			return View(viewModel);
        }

		public ActionResult SimilarProducts(int id)
		{
			var model = _productService.GetSimilarProducts(id);
			return PartialView("_SimilarProducts", model);
		}


		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Dashboard()
		{
            var userCount = await _userManager.Users.CountAsync();
			var productCount = await _context.Products.CountAsync();
            var soldOutCount = _productService.GetSoldOutProductsCount();
			var newOrders = _orderService.GetPendingOrderCount();		

			ViewBag.UserCount = userCount;
			ViewBag.ProductCount = productCount;
			ViewBag.SoldOutCount = soldOutCount;
			ViewBag.NewOrders = newOrders;

			return View();
		}


		public IActionResult Privacy()
		{
			return View();
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

    }
}
