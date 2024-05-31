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
        private readonly IWebHostEnvironment webHostEnvironment;

		
		public SubCategoriesController(
            ApplicationDbContext context,
            ISubCategoryService subCategoryService,
            IMainCategoryService mainCategoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this.subCategoryService = subCategoryService;
            this.mainCategoryService = mainCategoryService;
            this.webHostEnvironment = webHostEnvironment;
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
        public IActionResult Create(SubCategoryViewModel sCat)
        {
            try
            {
                string fileName = "";
                if (sCat.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                    fileName = Guid.NewGuid().ToString() + "_" + sCat.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    sCat.Image.CopyTo(new FileStream(filePath, FileMode.Create));


                    SubCategory subCategory = new SubCategory
                    {
                        Name = sCat.Name,
                        Subtitle = sCat.Subtitle,
                        Image = fileName,
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
                return View();
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
        public async Task<ActionResult> Edit(SubCategoryViewModel sCat)
        {
            try
            {

              /*  if (!ModelState.IsValid)
                {
                    sCat.MainCategories = mainCategoryService.GetMainCategories().ToList();
                    ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "Id", "Name", sCat.MainCategoryId);
                    return View(sCat);
                }*/

                string fileName = sCat.Image != null ? Guid.NewGuid().ToString() + "_" + sCat.Image.FileName : null;

                if (sCat.Image != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        sCat.Image.CopyToAsync(fileStream);
                    }
                }

                var subCategory = new SubCategory
                {
                    Id = sCat.Id,
                    Name = sCat.Name,
                    Subtitle = sCat.Subtitle,
                    Image = fileName ?? subCategoryService.GetSubCategoryById(sCat.Id).Image,
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
