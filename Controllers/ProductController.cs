using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceProject.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _context;
		private Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv;
		private readonly IProductService service;
		private readonly ISubCategoryService subCategoryService;
		private readonly IMainCategoryService mainCategoryService;
		private readonly IBrandService brandService;


		public ProductController(
			ApplicationDbContext _context,
			Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv,
			IProductService service,
			ISubCategoryService subCategoryService,
			IMainCategoryService mainCategoryService,
			IBrandService brandService)
		{
			this._context = _context;
			this._iHostEnv = _iHostEnv;
			this.service = service;
			this.subCategoryService = subCategoryService;
			this.mainCategoryService = mainCategoryService;
			this.brandService = brandService;
		}


		[Authorize(Roles = "Admin")]
		public ActionResult Index()
		{
			var model = service.GetProducts();
			return View(model);
		}


		[Authorize(Roles = "Admin")]
		public ActionResult SoldOutProducts()
		{
			var model = service.GetProducts();
			return View(model);
		}


		[Authorize(Roles = "Customer")]
		public ActionResult UserPage()
		{
			var model = service.GetProducts();
			return View(model);
		}


		[Authorize(Roles = "Admin")]
		public ActionResult Details(int id)
		{
			var model = service.GetProductById(id);
			return View(model);
		}

	
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
			ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
			ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");

			return View();
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Create(ProductViewModel product, IFormFile file)
		{
			try
			{
				using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/product\\" + file.FileName, FileMode.Create, FileAccess.Write))
				{
					file.CopyTo(fs);
				}

				product.ImagePath = "~/uploads/product/" + file.FileName;

				var pro = new Product
				{
					Name = product.Name,
					Description = product.Description,
					Price = product.Price,
					Stock = product.Stock,
					IsAvailable = product.IsAvailable,
					IsActive = product.IsActive,
					Image = product.ImagePath,
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
					product.Brands = brandService.GetBrands().ToList();

					return View(product);
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				//                ViewBag.ErrorMsg = "Something went wrong...";

				product.MainCategories = mainCategoryService.GetMainCategories().ToList();
				product.SubCategories = subCategoryService.GetSubCategories().ToList();
				product.Brands = brandService.GetBrands().ToList();

				return View(product);
			}
		}

		
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			var product = service.GetProductById(id);
			HttpContext.Session.SetString("oldImageUrl", product.Image);

			if (product == null)
			{
				return NotFound();
			}

			var viewModel = new ProductViewModel
			{
				ProductId = product.ProductId,
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Stock = product.Stock,
				IsAvailable = product.IsAvailable,
				IsActive = product.IsActive,
				ImagePath = product.Image,
				OfferPercentage = product.OfferPercentage,
				MainCategoryId = product.MainCategoryId,
				MainCategories = mainCategoryService.GetMainCategories().ToList(),
				SubCategoryId = product.SubCategoryId,
				SubCategories = subCategoryService.GetSubCategories().ToList(),
				BrandId = product.BrandId,
				Brands = brandService.GetBrands().ToList()
			};

			ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
			ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
			ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");

			return View(viewModel);
		}

		
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(ProductViewModel product, IFormFile file)
		{
			try
			{
				string oldimageurl = HttpContext.Session.GetString("oldImageUrl");

				if (file != null)
				{
					using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/product\\" + file.FileName, FileMode.Create, FileAccess.Write))
					{
						file.CopyTo(fs);
					}

					product.ImagePath = "~/uploads/product/" + file.FileName;

					string[] str = oldimageurl.Split("/");
					string str1 = (str[str.Length - 1]);
					string path = _iHostEnv.WebRootPath + "\\uploads/product\\" + str1;
					System.IO.File.Delete(path);
				}
				else
				{
					product.ImagePath = oldimageurl;
				}

				Product pro = new Product
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Description = product.Description,
					Price = product.Price,
					Stock = product.Stock,
					IsAvailable = product.IsAvailable,
					IsActive = product.IsActive,
					Image = product.ImagePath,
					OfferPercentage = product.OfferPercentage,
					MainCategoryId = product.MainCategoryId,
					SubCategoryId = product.SubCategoryId,
					BrandId = product.BrandId
				};

				int result = service.EditProduct(pro);

				if (result >= 1)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.ErrorMsg = "Something went wrong...";

					product.MainCategories = mainCategoryService.GetMainCategories().ToList();
					product.SubCategories = subCategoryService.GetSubCategories().ToList();
					product.Brands = brandService.GetBrands().ToList();

					ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
					ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
					ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");

					return View(product);
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;

				product.MainCategories = mainCategoryService.GetMainCategories().ToList();
				product.SubCategories = subCategoryService.GetSubCategories().ToList();
				product.Brands = brandService.GetBrands().ToList();

				ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
				ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
				ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");

				return View(product);
			}
		}


		// GET: ProductsController/Delete/5
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			var product = service.GetProductById(id);
			return View(product);
		}


		// POST: ProductsController/Delete/5
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				int resut = service.DeleteProduct(id);

				if (resut >= 1)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.ErrorMsg = "Somthing went wrong...";
					return View();
				}
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMsg = ex.Message;
				return View();
			}
		}


		[Authorize(Roles = "Admin")]
		public JsonResult GetSubCategories(int mainCategoryId)
		{
			var sCat = subCategoryService.GetSubCategoriesByMainCategoryId(mainCategoryId);
			return Json(sCat);
		}
	}
}