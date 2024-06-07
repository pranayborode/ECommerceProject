using ECommerceProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceProject.ViewModels;
using ECommerceProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceProject.Data;
using Microsoft.AspNetCore.Authorization;
namespace ECommerceProject.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISubCategoryService subCategoryService;
        private readonly IMainCategoryService mainCategoryService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv;


        public SubCategoriesController(
            ApplicationDbContext context,
            ISubCategoryService subCategoryService,
            IMainCategoryService mainCategoryService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv)
        {
            this._context = context;
            this.subCategoryService = subCategoryService;
            this.mainCategoryService = mainCategoryService;
            this._iHostEnv = _iHostEnv;
        }

        // GET: SubCategoriesController
        public ActionResult Index()
        {
            var model = subCategoryService.GetSubCategories();
            return View(model);
        }

        // GET: SubCategoriesController/Details/5
        public ActionResult Details(int id)
        {
            var model = subCategoryService.GetSubCategoryById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: SubCategoriesController/Create
        public IActionResult Create()
        {
           /* var viewModel = new SubCategoryViewModel 
            {
                MainCategories = mainCategoryService.GetMainCategories().ToList()

            };*/
            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
            return View();
        }

        // POST: SubCategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubCategoryViewModel sCat, IFormFile file)
        {
            try
            {
                using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/category\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                sCat.ImagePath = "~/uploads/category/" + file.FileName;
            


                    SubCategory subCategory = new SubCategory
                    {
                        Name = sCat.Name,
                        Subtitle = sCat.Subtitle,
                        Image =sCat.ImagePath,
                        MainCategoryId = sCat.MainCategoryId
                    };


                    int result = subCategoryService.AddSubCategory(subCategory);

                    if (result >= 1)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Something went wrong...";
                        sCat.MainCategories = mainCategoryService.GetMainCategories().ToList();
                        return View(sCat);
                    }

              
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: SubCategoriesController/Edit/5
        public ActionResult Edit(int id)
        {
            var subCategory = subCategoryService.GetSubCategoryById(id);
			HttpContext.Session.SetString("oldImageUrl", subCategory.Image);
			if (subCategory == null)
            {
                return NotFound();
            }

            var viewModel = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                Subtitle = subCategory.Subtitle,
                MainCategoryId = subCategory.MainCategoryId,
                MainCategories = mainCategoryService.GetMainCategories().ToList(),
                ImagePath = subCategory.Image
            };
            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name");
            return View(viewModel);
        }

        // POST: SubCategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SubCategoryViewModel sCat, IFormFile file)
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
					sCat.ImagePath = "~/uploads/category/" + file.FileName;

					string[] str = oldimageurl.Split("/");
					string str1 = (str[str.Length - 1]);
					string path = _iHostEnv.WebRootPath + "\\uploads/category\\" + str1;
					System.IO.File.Delete(path);
                }
                else
                {
                    sCat.ImagePath = oldimageurl;
                }

                var subCategory = new SubCategory
                {
                    Id = sCat.Id,
                    Name = sCat.Name,
                    Subtitle = sCat.Subtitle,
                    Image = sCat.ImagePath,
                    MainCategoryId = sCat.MainCategoryId
                };

                int result = subCategoryService.EditSubCategory(subCategory);

                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong...";
                    sCat.MainCategories = mainCategoryService.GetMainCategories().ToList();
                    ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name", sCat.MainCategoryId);
                    return View(sCat);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                sCat.MainCategories = mainCategoryService.GetMainCategories().ToList();
                ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name", sCat.MainCategoryId);
                return View(sCat);
            }
        }

        // GET: SubCategoriesController/Delete/5
        public ActionResult Delete(int id)
        {
            var sCat = subCategoryService.GetSubCategoryById(id);
            return View(sCat);
        }

        // POST: SubCategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                int resut = subCategoryService.DeleteSubCategory(id);
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
