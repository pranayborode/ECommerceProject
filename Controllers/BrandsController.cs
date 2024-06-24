using System;
using Microsoft.AspNetCore.Mvc;
using ECommerceProject.Data;
using ECommerceProject.Models;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ECommerceProject.Services;


namespace ECommerceProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBrandService service;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv;

        public BrandsController(
            ApplicationDbContext context,
            IBrandService service,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostEnv)
        {
            _context = context;
            this.service = service;
            this._iHostEnv = _iHostEnv;

        }

        public ActionResult Index()
        {
            var model = service.GetBrands();
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var model = service.GetBrandById(id);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BrandViewModel brandViewModel, IFormFile file)
        {
            try
            {
                using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/brand\\" + file.FileName, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                brandViewModel.ImagePath = "~/uploads/brand/" + file.FileName;

                var _brand = new Brand
                {
                    Name = brandViewModel.Name,
                    Image = brandViewModel.ImagePath
                };


                int result = service.AddBrand(_brand);

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
            var brand = service.GetBrandById(id);
            HttpContext.Session.SetString("oldImageUrl", brand.Image);
            if (brand == null)
            {
                return NotFound();
            }
            var viewModel = new BrandViewModel
            {
                BrandId = brand.BrandId,
                Name = brand.Name,
                ImagePath = brand.Image
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandViewModel brandViewModel, IFormFile file)
        {
            try
            {
                string oldImageUrl = HttpContext.Session.GetString("oldImageUrl");

                if (file != null)
                {
                    using (var fs = new FileStream(_iHostEnv.WebRootPath + "\\uploads/brand\\" + file.FileName, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }

                    brandViewModel.ImagePath = "~/uploads/brand/" + file.FileName;

                    string[] str = oldImageUrl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = _iHostEnv.WebRootPath + "\\uploads/brand\\" + str1;
                    System.IO.File.Delete(path);
                }
                else
                {
                    brandViewModel.ImagePath = oldImageUrl;
                }

                Brand brand = new Brand
                {
                    BrandId = brandViewModel.BrandId,
                    Name = brandViewModel.Name,
                    Image = brandViewModel.ImagePath
                };

                int result = service.EditBrand(brand);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.ErrorMsg = "Something went wrong...";
                    return View(brandViewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(brandViewModel);
            }


        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = service.GetBrandById(id);
            HttpContext.Session.SetString("oldImageUrl", model.Image);
            return View(model);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                string oldImageUrl = HttpContext.Session.GetString("oldImageUrl");

                if (id == null || service.GetBrands == null)
                {
                    return NotFound();
                }

                var result = service.DeleteBrand(id);

                if (result >= 1)
                {
                    string[] str = oldImageUrl.Split("/");
                    string str1 = (str[str.Length - 1]);
                    string path = _iHostEnv.WebRootPath + "\\uploads/brand\\" + str1;
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

        private bool BrandExists(int id)
        {
            return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        }
    }
}