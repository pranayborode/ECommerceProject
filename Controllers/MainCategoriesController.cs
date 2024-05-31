using ECommerceProject.Models;
using ECommerceProject.Services;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ECommerceProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class MainCategoriesController : Controller
	{

		private readonly IMainCategoryService service;
		private readonly IWebHostEnvironment webHostEnvironment;

		public MainCategoriesController(
			IMainCategoryService service,
			IWebHostEnvironment webHostEnvironment)
        {
            this.service = service;
			this.webHostEnvironment = webHostEnvironment;
        }

        // GET: MainCategoriesController
        public ActionResult Index()
		{
			var model = service.GetMainCategories();
			return View(model);
		}

		// GET: MainCategoriesController/Details/5
		public ActionResult Details(int id)
		{
			var model = service.GetMainCategoryById(id);
			return View(model);
		}

		// GET: MainCategoriesController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: MainCategoriesController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(MainCategoryViewModel mCat)
		{
			try
			{
				string fileName = "";
				if (mCat.Image != null)
				{
					string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
					fileName = Guid.NewGuid().ToString() + "_" + mCat.Image.FileName;
					string filePath = Path.Combine(uploadsFolder, fileName);
					mCat.Image.CopyTo(new FileStream(filePath, FileMode.Create));


					MainCategory mainCategory = new MainCategory
					{
						Name = mCat.Name,
						Subtitle = mCat.Subtitle,
						Image = fileName
					};


					int result = service.AddMainCategory(mainCategory);
				
					if (result >= 1)
					{
						return RedirectToAction(nameof(Index));
					}
					else
					{
						ViewBag.ErrorMsg = "Something went wrong...";
						return View();
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

        // GET: MainCategoriesController/Edit/5
        public async Task<IActionResult> Edit(int id)
		{
            var mCat = service.GetMainCategoryById(id);
            if (mCat == null)
            {
                return NotFound();
            }

			
			var viewModel = new MainCategoryViewModel
			{
				Id = mCat.Id,
				Name = mCat.Name,
				Subtitle = mCat.Subtitle,
				ImagePath = mCat.Image
            };

            return View(viewModel);


           
		}

		// POST: MainCategoriesController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MainCategoryViewModel mCat)
		{
            try
            {
                string fileName = mCat.Image != null ? Guid.NewGuid().ToString() + "_" + mCat.Image.FileName : null;

                if (mCat.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await mCat.Image.CopyToAsync(fileStream);
                    }
                }

                var mainCategory = new MainCategory
                {
                    Id = mCat.Id,
                    Name = mCat.Name,
                    Subtitle = mCat.Subtitle,
                    Image = fileName ?? service.GetMainCategoryById(mCat.Id).Image
                };

                int result = service.EditMainCategory(mainCategory);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong...";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
        }

		// GET: MainCategoriesController/Delete/5
		public ActionResult Delete(int id)
		{
			var mCat = service.GetMainCategoryById(id);
			return View(mCat);
		}

		// POST: MainCategoriesController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				int resut = service.DeleteMainCategory(id);
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
	}
}
