using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceProject.Data;

namespace ECommerceProject.Controllers
{
	public class ProductController : Controller
	{

		private readonly IProductService service;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly ApplicationDbContext _context;
		private readonly ISubCategoryService subCategoryService;
		private readonly IMainCategoryService mainCategoryService;

		

		public ProductController(
			IProductService service,
			IWebHostEnvironment webHostEnvironment,
			ISubCategoryService subCategoryService,
			IMainCategoryService mainCategoryService,
            ApplicationDbContext context)
        {
			this.service = service;
			this.webHostEnvironment = webHostEnvironment;	
			this.subCategoryService = subCategoryService;
		    this.mainCategoryService = mainCategoryService;
			_context = context;
        }

        // GET: ProductsController
        public ActionResult Index()
		{
			var model = service.GetProducts();
			return View(model);
		}

		// GET: ProductsController/Details/5
		public ActionResult Details(int id)
		{
			var model = service.GetProductById(id);
			return View(model);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
		{
            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");
            return View();
		}

		// POST: ProductsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ProductViewModel product)
		{
			try
			{
				string fileName = "";
				if (product.Image!=null)
				{
					string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/product");
					fileName = Guid.NewGuid().ToString() + "_" + product.Image.FileName;
					string filePath = Path.Combine(uploadsFolder, fileName);
					product.Image.CopyTo(new FileStream(filePath, FileMode.Create));


					var pro = new Product
					{
						Name = product.Name,
						Description = product.Description,
						Price = product.Price,
						Stock = product.Stock,
						IsAvailable = product.IsAvailable,
						SKU = product.SKU,
						Image = fileName,
						OfferPercentage = product.OfferPercentage,
						MainCategoryId = product.MainCategoryId,
						SubCategoryId = product.SubCategoryId,
						BrandId = product.BrandId
					};
					int result = service.AddProduct(pro);

					if (result >= 1)
					{
						return RedirectToAction(nameof(Index));
					}
					else
					{
						ViewBag.ErrorMsg = "Something went wrong...";
						product.MainCategories = mainCategoryService.GetMainCategories().ToList();
						product.SubCategories = subCategoryService.GetSubCategories().ToList();

						return View(product);
					}
				}

				return View();

			}
			catch (Exception ex)
			{

				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

		// GET: ProductsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ProductsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductsController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ProductsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
