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
		private readonly IProductService service;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly ISubCategoryService subCategoryService;
		private readonly IMainCategoryService mainCategoryService;
		private readonly IBrandService brandService;



		public ProductController(
			ApplicationDbContext _context,
			IProductService service,
			IWebHostEnvironment webHostEnvironment,
			ISubCategoryService subCategoryService,
			IMainCategoryService mainCategoryService,
			IBrandService brandService)
		{
			this._context = _context;
			this.service = service;
			this.webHostEnvironment = webHostEnvironment;
			this.subCategoryService = subCategoryService;
			this.mainCategoryService = mainCategoryService;
			this.brandService = brandService;

		}


		// GET: ProductsController
		[Authorize(Roles = "Admin")]
		public ActionResult Index()
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

		// GET: ProductsController/Details/5
		[Authorize(Roles = "Admin")]
		public ActionResult Details(int id)
		{
			var model = service.GetProductById(id);
			return View(model);
		}

		// GET: ProductsController/Create
		[Authorize(Roles = "Admin")]
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
		[Authorize(Roles = "Admin")]
		public ActionResult Create(ProductViewModel product)
		{
			try
			{
				string fileName = "";
				if (product.Image != null)
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
						product.Brands = brandService.GetBrands().ToList();
						return View(product);
					}
				}
				else
				{
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

		// GET: ProductsController/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
		{
			var product = service.GetProductById(id);
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

		// POST: ProductsController/Edit/5
		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(ProductViewModel product)
		{
			try
			{

				
				string fileName = product.Image != null ? Guid.NewGuid().ToString() + "_" +product.Image.FileName : null;

                if (product.Image != null)
				{
					string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/product");
					string filePath = Path.Combine(uploadsFolder, fileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						product.Image.CopyToAsync(fileStream);
					}
				}

				Product pro = new Product
				{
					ProductId = product.ProductId,
					Name = product.Name,
					Description = product.Description,
					Price = product.Price,
					Stock = product.Stock,
					IsAvailable = product.IsAvailable,
					
					Image = fileName ?? service.GetProductById(product.ProductId).Image,
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
