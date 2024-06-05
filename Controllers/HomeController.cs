using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommerceProject.Controllers
{
    [Authorize(Roles = "Customer,Admin")]
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
		private readonly ApplicationDbContext _context;

		public HomeController(
			ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
			ApplicationDbContext context)
		{
			_logger = logger;
			_userManager = userManager;
			_context = context;
		}
        [AllowAnonymous]
        public IActionResult Index()
		{
			return View();
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Dashboard()
		{
            var userCount = await _userManager.Users.CountAsync();
			var productCount = await _context.Products.CountAsync();

			ViewBag.UserCount = userCount;
			ViewBag.ProductCount = productCount;
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
