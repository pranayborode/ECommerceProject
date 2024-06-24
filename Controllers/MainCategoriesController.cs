﻿using ECommerceProject.Models;
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
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv;

        public MainCategoriesController(
			IMainCategoryService service,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv)
        {
            this.service = service;
			this._iHostEnv = _iHostEnv;
        }

    
        public ActionResult Index()
		{
			var model = service.GetMainCategories();
			return View(model);
		}

	
		public ActionResult Details(int id)
		{
			var model = service.GetMainCategoryById(id);
			return View(model);
		}

		
		public ActionResult Create()
		{
			return View();
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(MainCategoryViewModel mCat, IFormFile file)
		{
			try
			{
                using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/category\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                mCat.ImagePath = "~/uploads/category/" + file.FileName;

                MainCategory mainCategory = new MainCategory
					{
						Name = mCat.Name,
						Subtitle = mCat.Subtitle,
						Image = mCat.ImagePath
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
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

       
        public async Task<IActionResult> Edit(int id)
		{
            var mCat = service.GetMainCategoryById(id);
            HttpContext.Session.SetString("oldImageUrl", mCat.Image);

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

		
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MainCategoryViewModel mCat,IFormFile file)
		{
            try
            {
                string oldimageurl = HttpContext.Session.GetString("oldImageUrl");

                if (file != null)
                {
                    using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/category\\" + file.FileName, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }

                    mCat.ImagePath = "~/uploads/category/" + file.FileName;

                    string[] str = oldimageurl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = _iHostEnv.WebRootPath + "\\uploads/category\\" + str1;
                    System.IO.File.Delete(path);

				}
				else
				{
                    mCat.ImagePath = oldimageurl;
                }

                var mainCategory = new MainCategory
                {
                    Id = mCat.Id,
                    Name = mCat.Name,
                    Subtitle = mCat.Subtitle,
                    Image = mCat.ImagePath
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


		public ActionResult Delete(int id)
		{
			var mCat = service.GetMainCategoryById(id);
            HttpContext.Session.SetString("oldImageUrl", mCat.Image);

            return View(mCat);
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
                string oldImageUrl = HttpContext.Session.GetString("oldImageUrl");

                int resut = service.DeleteMainCategory(id);
				if (resut >= 1)
				{
                    string[] str = oldImageUrl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = _iHostEnv.WebRootPath + "\\uploads/category\\" + str1;
                    System.IO.File.Delete(path);

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
